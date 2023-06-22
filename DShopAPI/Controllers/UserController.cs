using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DShopAPI.Data;
using DShopAPI.Models;
using DShopAPI.ViewModels;
using DShopAPI.ViewModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BCrypt.Net;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DShopDbContext _dbContext;
        private readonly SmtpSettings _smtpSettings;

        public UserController(DShopDbContext dbContext, IOptions<SmtpSettings> smtpSettings)
        {
            _dbContext = dbContext;
            _smtpSettings = smtpSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userDto)
        {
            try
            {
                // Check if email or username already exists
                if (_dbContext.Users.Any(u => u.Email == userDto.Email || u.UserName == userDto.UserName))
                {
                    return BadRequest("Email or username already exists.");
                }

                // Generate OTP
                string otp = GenerateOTP();

                // Save the user to the database
                var user = new Users
                {
                    Email = userDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    PhoneNumber = userDto.PhoneNumber,
                    UserName = userDto.UserName,
                    ConfirmationCode = otp,
                    VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(10) // Set verification code expiration time
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                // Send OTP to user's email
                bool otpSent = SendOTP(userDto.Email, otp);

                if (!otpSent)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send OTP. Please try again later.");
                }

                return Ok("Check your email"); // Redirect to the verification page
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerificationDto verificationDto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.ConfirmationCode == verificationDto.Code);

            if (user == null)
            {
                return BadRequest("Incorrect verification code.");
            }

            // Check if the verification code has expired
            if (user.VerificationCodeExpiration < DateTime.UtcNow)
            {
                return BadRequest("Verification code has expired. Please request a new one.");
            }

            user.ConfirmationCode = null; // Code verified, set it to null
            await _dbContext.SaveChangesAsync();

            // Add a message to the email body to inform the user about successful verification
            string emailBody = "Your verification is successful. You can now proceed with your login.";

            // Send the email to notify the user about successful verification
            bool emailSent = SendEmail(user.Email, "Verification Successful", emailBody);

            if (!emailSent)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send email notification. Please try again later.");
            }

            return Redirect("/LoginLogout.html"); // Redirect to the login page
        }

        private bool SendEmail(string email, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = true;

                mail.From = new MailAddress(_smtpSettings.Username);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
        }


        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userDto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == userDto.Email);

            if (user == null)
            {
                return Unauthorized("User does not exist.");
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                return Unauthorized("Incorrect password.");
            }

            // Redirect to the home page or return a token for authentication
            return Redirect("/index.html");
        }

        [HttpPost("resendotp")]
        public IActionResult ResendOTP(string email)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            string otp = GenerateOTP();

            user.ConfirmationCode = otp;
            _dbContext.SaveChanges();

            // Send OTP to user's email
            bool otpSent = SendOTP(user.Email, otp);

            if (!otpSent)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send OTP. Please try again later.");
            }

            return Ok("OTP resent successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return Ok("User deleted successfully.");
        }


        private bool SendOTP(string email, string otp)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = true;

                mail.From = new MailAddress(_smtpSettings.Username);
                mail.To.Add(email);
                mail.Subject = "OTP Verification";
                mail.Body = $"Your OTP is: {otp}";

                smtpClient.Send(mail);

                // Set the verification code expiration time to 10 minutes from now
                TimeSpan codeExpirationTime = TimeSpan.FromMinutes(10);
                var user = _dbContext.Users.SingleOrDefault(u => u.Email == email);
                if (user != null)
                {
                    user.ConfirmationCode = otp;
                    user.VerificationCodeExpiration = DateTime.UtcNow.Add(codeExpirationTime);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
        }


        private string GenerateOTP()
        {
            // Generate a 6-digit OTP
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }
    }
}
