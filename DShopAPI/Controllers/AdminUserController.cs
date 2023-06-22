using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DShopAPI.Data;
using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.ViewModels;
using DShopAPI.ViewModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BCrypt.Net;
using DShopAPI.Interfaces;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly SmtpSettings _smtpSettings;

        public AdminUserController(IAdminUserRepository adminUserRepository, IOptions<SmtpSettings> smtpSettings)
        {
            _adminUserRepository = adminUserRepository;
            _smtpSettings = smtpSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminUserRegistrationDto userDto)
        {
            try
            {
                // Check if email or username already exists
                if (_adminUserRepository.GetAdminUserByEmail(userDto.Email) != null ||
                    _adminUserRepository.GetAdminUserByUsername(userDto.UserName) != null)
                {
                    return BadRequest("Email or username already exists.");
                }

                // Generate AdminId
                string adminId = GenerateAdminId();

                // Save the user to the database
                var user = new AdminUser
                {
                    Email = userDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    PhoneNumber = userDto.PhoneNumber,
                    UserName = userDto.UserName,
                    AdminId = adminId
                };

                _adminUserRepository.AddAdminUser(user);
                _adminUserRepository.SaveChanges();

                // Send registration email to admin user
                bool emailSent = SendRegistrationEmail(userDto.Email, adminId);

                if (!emailSent)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send registration email. Please try again later.");
                }

                return Ok($"Admin user created successfully. AdminId: {adminId}");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(AdminUserLoginDto userDto)
        {
            var user = _adminUserRepository.GetAdminUserByAdminId(userDto.AdminId);

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


        [HttpDelete("{id}")]
        public IActionResult DeleteAdminUser(int id)
        {
            var user = _adminUserRepository.GetAdminUserById(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            _adminUserRepository.DeleteAdminUser(user);
            _adminUserRepository.SaveChanges();

            return Ok("Admin user deleted successfully.");
        }

        private bool SendRegistrationEmail(string email, string adminId)
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
                mail.Subject = "Admin User Registration";
                mail.Body = $"Dear Admin,\n\nThank you for registering as an admin user.\n\nYour AdminId: {adminId}\n\nPlease use this AdminId along with your password to log in.\n\nFor more information about the admin functionalities, please refer to the following video:\n\nVideo Link: {GetAdminFunctionalitiesVideoLink()}";

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
        }

        private string GenerateAdminId()
        {
            // Generate a unique admin ID (e.g., using a GUID)
            return Guid.NewGuid().ToString();
        }

        private string GetAdminFunctionalitiesVideoLink()
        {
            // Provide the actual video link for admin functionalities
            return "https://www.youtube.com/watch?v=VZJ3hoVx6-o&pp=ygUUYWRtaW4gZnVuY3Rpb25hbGl0eSA%3D";
        }
    }
}
