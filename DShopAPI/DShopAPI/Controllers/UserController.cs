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
                    Password = userDto.Password,
                    PhoneNumber = userDto.PhoneNumber,
                    UserName = userDto.UserName,
                    ConfirmationCode = otp
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                // Send OTP to user's email
                bool otpSent = SendOTP(userDto.Email, otp);

                if (!otpSent)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send OTP. Please try again later.");
                }

                return Redirect("/verifyme.html"); // Redirect to the verification page
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userDto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == userDto.Email && u.Password == userDto.Password);

            if (user == null)
            {
                return Unauthorized("Incorrect email or password.");
            }

            // Redirect to the home page or return a token for authentication
            return Redirect("/index.html");
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
