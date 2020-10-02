using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace WeiXinOA.Models.Services
{
    public class WXMailService
    {
        private string _mailFrom;
        private string _mailTo;
        private string _password;
        private string _account;
        private string _smtp = "smtp.qq.com";
        private int _port = 465;

        public WXMailService()
        {
            _mailFrom = WXHostEnvHelper.GetWXOAConfigValue("Mail", "From");
            _mailTo = WXHostEnvHelper.GetWXOAConfigValue("Mail", "To");
            _password = WXHostEnvHelper.GetWXOAConfigValue("Mail", "Password");
            _account = WXHostEnvHelper.GetWXOAConfigValue("Mail", "Account");
        }


        public async Task SendMail(string subject, string body)
        {
            using SmtpClient smtp = new SmtpClient();
            var mailMessage = new MimeMessage();

            try
            {
                mailMessage.From.Add(new MailboxAddress(_mailFrom));
                mailMessage.To.Add(new MailboxAddress(_mailTo));
                mailMessage.Subject = subject;
                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = body };

                smtp.CheckCertificateRevocation = false; //防止出现验证错误

                //连接邮箱，验证账号密码，发送邮件，关闭链接
                await smtp.ConnectAsync(_smtp, _port, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_account, _password);
                await smtp.SendAsync(mailMessage);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception)
            {
                return;
            }
        }


    }
}
