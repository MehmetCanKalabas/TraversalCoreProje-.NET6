using EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using TraversalCoreProje.Areas.Admin.Models;
using TraversalCoreProje.Models;

namespace TraversalCoreProje.Controllers
{
	public class PasswordChangeController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
        public PasswordChangeController(UserManager<AppUser> userManager)
        {
			_userManager = userManager;
		}

		[HttpGet]
        public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel p)
		{
			var user = await _userManager.FindByEmailAsync(p.Mail);
			string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
			var passwordResetTokenLink = Url.Action("ResetPassword", "PasswordChange", new
			{
				userId = user.Id,
				token = passwordResetToken
			},HttpContext.Request.Scheme);

			MimeMessage mimeMessage = new MimeMessage();
			MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "mehmetcankalabas.mck@gmail.com");
			mimeMessage.From.Add(mailboxAddressFrom);

			MailboxAddress mailboxAddressTo = new MailboxAddress("User", p.Mail);
			mimeMessage.To.Add(mailboxAddressTo);

			var bodyBuilder = new BodyBuilder();
			bodyBuilder.TextBody = passwordResetTokenLink;
			mimeMessage.Body = bodyBuilder.ToMessageBody();

			mimeMessage.Subject = "Şifre değişiklik talebi";

			SmtpClient client = new SmtpClient();
			client.Connect("smtp.gmail.com", 587, false);
			client.Authenticate("mehmetcankalabas.mck@gmail.com", "şifre");
			client.Send(mimeMessage);
			client.Disconnect(true);

			return View();
		}

        [HttpGet]
        public IActionResult ResetPassword(string userid,string token)
		{
			TempData["userid"] = userid;
			TempData["token"] = token;
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel p)
        {
			var userid = TempData["userid"];
			var token = TempData["token"];
			if (userid==null || token == null)
			{
				//hata messajı
			}
			var user = await _userManager.FindByIdAsync(userid.ToString());
			var result = await _userManager.ResetPasswordAsync(user, token.ToString(), p.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("SignIn", "Login");
			}
            return View();
        }
    }
}