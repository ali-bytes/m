using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using WoodStore.Domain.Uow;

namespace WoodStore.Domain.Helpers
{
    public class ClsEmail: IDisposable
    {
        UnitOfWork uow = new UnitOfWork();

        private static string _logo =
            "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAAAoCAMAAACMwkUuAAAAk1BMVEUAAABJNJLwZyZJNJLwZybwZyZJNJJJNJLwZyZJNJLwZyZJNJLwZybwZyZJNJJJNJLwZyZJNJLwZyZJNJLwZyZJNJLwZyZJNJLwZyZJNJLwZyZJNJLwZyZJNJLwZybxcTTyekHzhE/0jVz1l2r2oHf3qoX4s5P4vaD5xq760Lv72cn849b97OT+9vH///9JNJLwZybnQZ6EAAAAH3RSTlMAEBAgIDAwQEBQUGBgcHCAgI+Pn5+vr7+/z8/f3+/vdkdMmQAABHJJREFUeF7Ul/Fy2yAMxiH0IC49CHM8wpj7Cs77P93SRCApkPZ6Zevt+ysIS+FnPhQiPpA0VLo7R0K7/VW3gbpNyzahX14KIsMkvqy0MWUnUDMELYZO56t+XQf+Ng2rwMCGRQyv7pUAhY1pneXXOMx2rxknVwgtGDqDDl2QDAnpYf3VYW2uJAeDbJMATTUkG5DXXQdE1QT14Yva2onRIAt3FvfWuejQAXE1wb1T3yIIlxwMsqFRkK0BeX1qQRL65J36q3wAYv4SiO69rHPVsQFRJEG9U98+AHEDQJJ5U2IggX83gKCe7kEcXRSvn733Idf9RZDJXATf5AeARNo9q7NQSwfk5z3IQhJSp76M4C0CAg8NBgkURHUP4plobxmIZAma12dmvQeZBoLYeBEWR6NA0HZATiJTEMACf4YGBFcvCUi8aB132KNvbJSgOgQRBPVs6QIWlpB7IBEeISC0m40FyZI6K5WjIjsgv0UmIJBeVqs/C6LF4B2x1FleBOYtIACSF4sgYPS5ZIbPgiQ5GGRT1ChlgZGCnI4AtMsVZC7AGr31KZDNjwZx2IJW/EpFQXavcFGxFWStFsxoFQShz6g+SB4BosxFxU7YghbcG0dBxKHcHSOAwMYl7OKhAZGd9msumsrgC+q1R09Wj0yJgQg4JgcDIGX1yJSb+r4DctVoEAUg3E8KPxOQ53JRiTeQXOyPyZrX1wHisQUZZq0tkh8mT17qSo8OguB/xaPZDP3RTpgQCEjcaLwPEkfffn1tQbMm4cRB9nVLDDprkezw9kHUvwIxtb1MjsYVAxFH+GwMOstNNEF3QWbxAGQeDLLin1y50AnHQZ6gBe/JOdKBJoQeSJKPQPxgEFuNkmBnQImDiAMOXDFT5q8EQTjHeBDZ7IdFo3jN5xQH2UELfq43zFnxhOkeJMNix1tLTJFqcfISUzDSNjJNQpyu+nHLfSl3R1Ee0DzBCgiEEGOcvRGgmkEHi/geYQt+Ef+79uXuKL5buz/FltGK4zAMRZdlCmWZgT4sQ1iGylZsre3YF/3/163llMm0D0shHXohurL04mOEyHGXDpcV/H56hF53Pek+Hc+6yfPqLsiaUZBA/cw9+BC9Kq/qbn1ejWaZqfv0RJDppJsKhgV0Vd+TZolTAWuyYlas6q46j7ZXb978PpCX0069fNyCMBamhKIOjTUiG4hHJkpg5oTIrIC1A1m7NKczsoE8Xq/3D/YtSAb3WOH+QuxYDISxzOrt2WW0AU0IPZGiBeK0pO8BmfRevU03IG3EBC5gy2jcfQFQWL+AFJB+jmIVejbI+dcNCEYUA3HdAMuJ5lQBvgLRVd7Fgu8arcPxbv38fQ1S4caA+YygLGWARIypki8gGXNPWBRJ1VXsBdm/Fo7XIIJMGlDtc0rLAPEopPEKZLa2q1WXxkq17QXZv6jfT58gJlpwWacJKA3Y1m9zG8hoV7TL+oU8H0Tf/qwepCsOjzQKucgsyuJUeRTNLYroVqIoYnTT039mfhwfosN/L/APHgxkleGky3gAAAAASUVORK5CYII=";
        public void SendEmail(string strTo, string strSubject, string strMessage, bool isHtml){
            var cfg = uow.EmailConfigRepository.GetAll().FirstOrDefault();
            if(cfg == null) return;

            var msg = new MailMessage{
                From = new MailAddress(cfg.MailFrom,"Petty cash System")
            };
        
            msg.To.Add(strTo);
            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = strMessage;
            var smtp = new SmtpClient(cfg.SmtpClient){
                Credentials = new NetworkCredential(cfg.UserName, cfg.Password),
                EnableSsl = cfg.SSL,
            };
            if(cfg.Port != null) smtp.Port = Convert.ToInt32(cfg.Port);
            smtp.Send(msg);
            msg.Dispose();
        }

        public void SendEmail(string strTo, string cc, string cc2, string strSubject, string strMessage, bool isHtml,string cc3=""){
            try
            {
            var cfg = uow.EmailConfigRepository.GetAll().FirstOrDefault();

            if(cfg == null) return;
            var msg = new MailMessage{
                From = new MailAddress(cfg.MailFrom, "Petty Cash System")
            };
            msg.To.Add(strTo);
            if (!string.IsNullOrEmpty(cc)) msg.CC.Add(new MailAddress(cc));
            if (!string.IsNullOrEmpty(cc2)) msg.CC.Add(new MailAddress(cc2));
            if (!string.IsNullOrEmpty(cc3)) msg.CC.Add(new MailAddress(cc3));
            msg.Subject = strSubject;
            msg.IsBodyHtml = isHtml;
            msg.Body = strMessage;
            var smtp = new SmtpClient(cfg.SmtpClient){
                EnableSsl = cfg.SSL,
                Credentials = new NetworkCredential(cfg.UserName, cfg.Password),
            };
            if(cfg.Port != null) smtp.Port = Convert.ToInt32(cfg.Port);
            
            smtp.Send(msg);
            msg.Dispose();
            }
            catch
            {

               
            }
        }
        public void SendPersonalEmail(string strTo, string cc, string cc2, string strSubject, string strMessage, bool isHtml, int UserID)
        {

            //var cfg = confg;//CnfgRepository.GetActiveCnfg();
            //if (cfg == null) return;
            //var msg = new MailMessage
            //{
            //    From = new MailAddress(cfg.PersonalMail, cfg.User.UserName)
            //};
            //msg.To.Add(strTo);
            //if (!string.IsNullOrEmpty(cc)) msg.CC.Add(new MailAddress(cc));
            //if (!string.IsNullOrEmpty(cc2)) msg.CC.Add(new MailAddress(cc2));
            //msg.Subject = strSubject;
            //msg.IsBodyHtml = isHtml;
            //msg.Body = strMessage;
            //var smtp = new SmtpClient(cfg.SmtpClient)
            //{
            //    EnableSsl = cfg.SSL,
            //    Credentials = new NetworkCredential(cfg.SenderUserName, cfg.SenderPassWord),
            //};
            //if (cfg.Port != 0) smtp.Port = Convert.ToInt32(cfg.Port);
            //smtp.Send(msg);
            //msg.Dispose();

            //var cfg = confg;//CnfgRepository.GetActiveCnfg();
            //if (cfg == null) return;
            //PersonalEmail confg = new PersonalEmail();
            //var personalemail = "";


            var cfg = uow.EmailConfigRepository.GetAll().FirstOrDefault();

            var userName = string.Empty;
            var pesonalEmail = string.Empty;
            var smtpclint = string.Empty;
            bool enableSSL;
            var senderUserName = string.Empty;
            var senderPassWord = string.Empty;
            var sendPort = 0;


            //using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
            //{


            //    cfg = context.PersonalEmails.FirstOrDefault(a => a.UserId == UserID);
            //    userName = cfg.User.UserName;
            //    pesonalEmail = cfg.PersonalMail;
            //    smtpclint = cfg.SmtpClient;
            //    enableSSL = cfg.SSL;
            //    senderUserName = cfg.SenderUserName;
            //    senderPassWord = cfg.SenderPassWord;
            //    sendPort = cfg.SendPort;
            //}
            //if (cfg == null) return;
            //var msg = new MailMessage
            //{
            //    From = new MailAddress(pesonalEmail, userName)
            //};
            //msg.To.Add(strTo);
            //if (!string.IsNullOrEmpty(cc)) msg.CC.Add(new MailAddress(cc));
            //if (!string.IsNullOrEmpty(cc2)) msg.CC.Add(new MailAddress(cc2));
            //msg.Subject = strSubject;
            //msg.IsBodyHtml = isHtml;
            //msg.Body = strMessage;
            
            //SmtpClient smtp = new SmtpClient();
            //smtp.UseDefaultCredentials = false;
            //smtp.Host = smtpclint;
            //if (enableSSL)
            //{
            //    smtp.EnableSsl = true;
            //}
            //else { smtp.EnableSsl = false; }
            //smtp.Credentials = new NetworkCredential(senderUserName, senderPassWord);
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //if (cfg.Port != 0) smtp.Port = Convert.ToInt32(sendPort);
            //smtp.Send(msg);
            //msg.Dispose();
             


           
        }
        //public static void SendReplayEmail(string strTo, string oldMessage, string olddate, string strSubject, string strMessage, bool isHtml, int UserID)
        //{
        //    PersonalEmail cfg = new PersonalEmail();
        //    var userName = string.Empty;
        //    var pesonalEmail = string.Empty;
        //    var smtpclint = string.Empty;
        //    bool enableSSL;
        //    var senderUserName = string.Empty;
        //    var senderPassWord = string.Empty;
        //    var sendPort = 0;
        //    StringBuilder newMessage = new StringBuilder("<b>" + strMessage + "</b>");
        //    newMessage.AppendLine("<hr>");
        //    newMessage.AppendLine(olddate + Environment.NewLine);
        //    newMessage.AppendLine(oldMessage);

        //    using (var context = new ISPDataContext(ConfigurationManager.AppSettings["ConnectionString"]))
        //    {


        //        cfg = context.PersonalEmails.FirstOrDefault(a => a.UserId == UserID);
        //        userName = cfg.User.UserName;
        //        pesonalEmail = cfg.PersonalMail;
        //        smtpclint = cfg.SmtpClient;
        //        enableSSL = cfg.SSL;
        //        senderUserName = cfg.SenderUserName;
        //        senderPassWord = cfg.SenderPassWord;
        //        sendPort = cfg.SendPort;
        //    }
        //    if (cfg == null) return;
        //    var msg = new MailMessage
        //    {
        //        From = new MailAddress(pesonalEmail, userName)
        //    };
        //    msg.To.Add(strTo);

        //    msg.Subject = strSubject;
        //    msg.IsBodyHtml = isHtml;
        //    msg.Body = newMessage.ToString();

        //    SmtpClient smtp = new SmtpClient();
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Host = smtpclint;
        //    if (enableSSL)
        //    {
        //        smtp.EnableSsl = true;
        //    }
        //    else { smtp.EnableSsl = false; }
        //    smtp.Credentials = new NetworkCredential(senderUserName, senderPassWord);
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

        //    if (cfg.Port != 0) smtp.Port = Convert.ToInt32(sendPort);
        //    smtp.Send(msg);
        //    msg.Dispose();
        //}
        //public static void SendEmailWithAttachment(string strTo, string strSubject, string strMessage,string attachPath, bool isHtml)
        //{
        //    var cfg = CnfgRepository.GetActiveCnfg();
        //    if (cfg == null) return;

        //    var msg = new MailMessage
        //    {
        //        From = new MailAddress(cfg.MailFrom, "Smart ISP System")
        //    };

        //    var attachFile = new Attachment(HttpContext.Current.Server.MapPath(attachPath));

        //    msg.To.Add(strTo);
        //    msg.Subject = strSubject;
        //    msg.IsBodyHtml = isHtml;
        //    msg.Body = strMessage;
        //    msg.Attachments.Add(attachFile);
        //    var smtp = new SmtpClient(cfg.SmtpClient)
        //    {
        //        Credentials = new NetworkCredential(cfg.UserName, cfg.Password),
        //        EnableSsl = cfg.SSL,
        //    };
        //    if (cfg.Port != null) smtp.Port = Convert.ToInt32(cfg.Port);
        //    smtp.Send(msg);
        //    msg.Dispose();
        //}
        public static string Body( string message)
        {
            //<h3 style='text-align: center'>Petty Cash System</h3>
            var body = new StringBuilder();
            body.Append(
                "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
                "<html xmlns='http://www.w3.org/1999/xhtml'> <head>" +
                "<meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />" +
                "<title></title>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0'/>" +
                "</head><body>");
            body.Append("<table width='100%'><tr><td align='center' bgcolor='#6fb3e0' style='padding: 40px 0 30px 0;'>" +
                        " <img src='http://i.imgur.com/cUoSysl.png' alt='logo'/></td></tr><tr><td bgcolor='#ffffff' style='padding: 40px 30px 40px 30px;text-align: right;'>" +
                        message +
                        "</td></tr><tr><td bgcolor='#438eb9' style='padding: 20px'><table width='100%'><tr><td>" +
                        "<p style='text-decoration: none;color: white;'>Copy Rights &copy; Petty Cash System 2016</p>" +
                        "</td>" +
                        "</tr></table></td></tr></table>");
            body.Append("</body></html>");
            return body.ToString();
        }




        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}


