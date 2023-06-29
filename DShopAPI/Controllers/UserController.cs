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
using Microsoft.EntityFrameworkCore;
using System.Web;

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

       
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                // Find the user by the reset token
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.ResetPasswordToken == resetPasswordDto.ResetToken);

                // Check if the user exists
                if (user == null)
                {
                    return BadRequest("Invalid reset token.");
                }

                // Check if the reset token has expired

                if (user.ResetPasswordTokenExpiration < DateTime.UtcNow)
                {
                    return BadRequest("Reset token has expired. Please request a new one.");
                }

                // Verify the old password
                if (!BCrypt.Net.BCrypt.Verify(resetPasswordDto.OldPassword, user.Password))
                {
                    return Unauthorized("Incorrect old password.");
                }

                // Validate the new password and confirm password
                if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
                {
                    return BadRequest("New password and confirm password do not match.");
                }

                // Update the user's password with the new hashed password
                user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                user.ResetPasswordToken = null; // Clear the reset token
                user.ResetPasswordTokenExpiration = null; // Clear the reset token expiration
                await _dbContext.SaveChangesAsync();

                // Send email notification to the user
                string emailBody = "Your password has been successfully changed.";
                bool emailSent = SendEmail(user.Email, "Password Reset Successful", emailBody);

                if (!emailSent)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send email. Please try again later.");
                }

                return Ok("Password reset successful. Please check your email for confirmation.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
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
            return Ok("Login Successful");
        }


        [HttpPost("reset/confirm")]
        public IActionResult ConfirmPasswordReset(ResetPasswordConfirmDto confirmDto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.ResetPasswordToken == confirmDto.ResetToken);

            if (user == null)
            {
                return NotFound("Invalid or expired password reset token.");
            }

            if (user.ResetPasswordTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired password reset token.");
            }

            // Update the user's password
            user.Password = BCrypt.Net.BCrypt.HashPassword(confirmDto.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;

            _dbContext.SaveChanges();

            // TODO: Send an email to the user notifying them that their password has been reset

            return Ok("Password reset successful. You can now log in with your new password.");
        }

        [HttpPost("reset/initiate")]
        public IActionResult InitiatePasswordReset(ResetPasswordInitiateDto initiateDto)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == initiateDto.Email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Generate a password reset token
            string resetToken = GenerateResetToken();

            // Set the password reset token and its expiration in the user model
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddMinutes(10);

            _dbContext.SaveChanges();

            // TODO: Send an email to the user containing the password reset token and a link to the password reset page
            string resetLink = $"https://bulwark.netlify.app/reset-password?token={HttpUtility.UrlEncode(resetToken)}";
            // Send the resetLink to the user's email using your email sending functionality

            return Ok("Password reset initiated. Please check your email for further instructions.");
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
        private bool SendPasswordResetEmail(string email, string resetToken)
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
                mail.Subject = "Password Reset";
                mail.Body = $"To reset your password, use the following token: {resetToken}";

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
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

        private string GenerateResetToken()
        {
            // Generate a random string for the reset token
            const int tokenLength = 64;
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string resetToken = new string(Enumerable.Repeat(allowedChars, tokenLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return resetToken;
        }
    }
}
