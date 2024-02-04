using P.DataAccessLayer.Models;
using System.Net;
using System.Net.Mail;

namespace P.PresentationLayer.Helpers
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("ahmedashraf192f@gmail.com", "zung sumh yqni xcol");
			Client.Send("ahmedashraf192f@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
