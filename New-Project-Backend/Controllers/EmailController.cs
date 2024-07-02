using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using New_Project_Backend.Model;
using Project.Core.CustomModels;
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
		public ActionResult SendEmail(EmailModel model)
		{
			try
			{
				// Configure SMTP client
				using (var client = new SmtpClient("smtp.gmail.com", 587))
				{
					string FilePath = "C:\\Users\\Mohan_Lalitha\\source\\repos\\New-Project-Backend\\New-Project-Backend\\email temp\\email.html";
					StreamReader str = new StreamReader(FilePath);
					string MailText = str.ReadToEnd();
					str.Close(); 

					MailText = MailText.Replace("{{name}}", model.Name);
					MailText = MailText.Replace("{{email}}", model.Email);
					MailText = MailText.Replace("{{phonenumber}}", model.PhoneNumber);
					MailText = MailText.Replace("{{description}}", model.Description);

					client.EnableSsl = true;
					client.Credentials = new NetworkCredential("mganesh120798@gmail.com", "arvp sber ydgt pncw");
					
					// Create message
					var mailMessage = new MailMessage();
					mailMessage.IsBodyHtml = true;
					mailMessage.From = new MailAddress("mganesh120798@gmail.com");
					mailMessage.To.Add(model.Email);
					mailMessage.Subject = "Form SuccessFully Submitted";
					mailMessage.Body = MailText;

					// Send email
					client.Send(mailMessage);
				}

				return Ok("Email Sent Successfully");
			}
			catch (Exception ex)
			{
				// Log the exception or handle it as per your application's needs
				throw;
			}
		}
	}
}
