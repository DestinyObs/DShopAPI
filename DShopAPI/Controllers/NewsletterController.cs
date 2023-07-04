using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DShopAPI.Interfaces;
using DShopAPI.Models;
using DShopAPI.Repositories;
using DShopAPI.ViewModels;
using DShopAPI.ViewModels.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly INewsletterSubscriberRepository _subscriberRepository;
        private readonly SmtpSettings _smtpSettings;

        public NewsletterController(INewsletterSubscriberRepository subscriberRepository, IOptions<SmtpSettings> smtpSettings)
        {
            _subscriberRepository = subscriberRepository;
            _smtpSettings = smtpSettings.Value;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToNewsletter(NewsletterSubscriberDto subscriberDto)
        {
            try
            {
                // Check if the email is already subscribed
                if (_subscriberRepository.GetSubscriberByEmail(subscriberDto.Email) != null)
                {
                    return BadRequest("Email is already subscribed.");
                }

                // Create a new newsletter subscriber
                var subscriber = new NewsletterSubscriber
                {
                    Email = subscriberDto.Email
                };

                _subscriberRepository.AddSubscriber(subscriber);
                _subscriberRepository.SaveChanges();

                // Send a confirmation email to the subscriber
                bool emailSent = SendConfirmationEmail(subscriber);

                if (!emailSent)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send confirmation email. Please try again later.");
                }

                return Ok("Subscribed to the newsletter successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool SendConfirmationEmail(NewsletterSubscriber subscriber)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = true;

                mail.From = new MailAddress(_smtpSettings.Username);
                mail.To.Add(subscriber.Email);
                mail.Subject = "Newsletter Subscription Confirmation";
                mail.Body = "Thank you for subscribing to our newsletter!";

                smtpClient.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return false;
            }
        }
    }
}
