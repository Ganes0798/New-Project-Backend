using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace New_Project_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        [Route("sendemail")]
        public IActionResult SendEmail(EmailModel model)
        {
            try
            {
                // Configure SMTP client
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    string filePath = "C:\\Users\\mgmga\\New-Project-Backend\\New-Project-Backend\\email temp\\email.html";
                    string mailText = System.IO.File.ReadAllText(filePath);

                    mailText = mailText.Replace("{{name}}", model.Name)
                                       .Replace("{{email}}", model.Email);

                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("mganesh120798@gmail.com", "gmci zafc snpj sjac");

                    // Create message
                    var mailMessage = new MailMessage
                    {
                        IsBodyHtml = true,
                        From = new MailAddress("mganesh120798@gmail.com"),
                        Subject = "Form Successfully Submitted",
                        Body = mailText
                    };

                    mailMessage.To.Add(model.Email);

                    // Send email
                    client.Send(mailMessage);
                }

                return Ok("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's needs
                // Example: _logger.LogError(ex, "An error occurred while sending the email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        [Route("sendemailto")]
        public IActionResult SendEmailToClient(EmailModel modelName)
        {
            try
            {
                // Configure SMTP client
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    string filePath = "C:\\Users\\mgmga\\New-Project-Backend\\New-Project-Backend\\email temp\\emailToClient.html";
                    string mailText = System.IO.File.ReadAllText(filePath);

                    mailText = mailText.Replace("{{name}}", modelName.Name)
                                       .Replace("{{email}}", modelName.Email);

                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("mganesh120798@gmail.com", "gmci zafc snpj sjac");

                    // Create message
                    var mailMessage = new MailMessage
                    {
                        IsBodyHtml = true,
                        From = new MailAddress("mganesh120798@gmail.com"),
                        Subject = "Form Send From " + modelName.Name,
                        Body = mailText
                    };

                    mailMessage.To.Add("mganesh120798@gmail.com");

                    // Send email
                    client.Send(mailMessage);
                }

                return Ok(ErrorCodes.BookAddedSuccessfully.ToString());

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's needs
                // Example: _logger.LogError(ex, "An error occurred while sending the email.");
                throw;
            }
        }
    }

       
}
