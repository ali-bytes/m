using System;
using System.Linq;
using WoodStore.Domain.Resources;
using WoodStore.Domain.Uow;

namespace WoodStore.Domain.Helpers
{
   public class EmailNotify
    {
       UnitOfWork uow = new UnitOfWork();
       ClsEmail em=new ClsEmail();
       public void AddRequestNotify(string userId, string requestSerial)
       {

           var user = uow.UserRepository.FindById(userId);
           var cashier = uow.UserRepository.Get(a => a.BranchId == user.BranchId && a.Roles.Any(r => r.Id == "deb8cf0f-2f8f-4dfa-b8b9-ca0be155e238")).FirstOrDefault();

           // send message to user
           var userMsg =
               "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
               Token.YouSendPettyCashRequestTo + "</span></div>" + cashier.FullName +
               "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
               "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
               "</h3><h3 ><div style='font-weight: bold; color: blue; '>"  + Token.PettyCashRequestSerialEmail +
               "</div> <span> <br/>" + requestSerial +
               "</h3></div>";


           var formalmessage = ClsEmail.Body(userMsg);
           em.SendEmail(user.NotificationEmail, null, null, "" + Token.User + ": " + user.FullName, formalmessage, true);

           // send message to cashier
           var cashierMsg =
              "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
              Token.YouHaveWaitingApprovalPettyCashRequestFrom + "</span></div>" + user.FullName +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
              "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.PettyCashRequestSerialEmail +
              "</div> <span> <br/>" + requestSerial +
              "</h3></div>";

           var cashierFormalmessage = ClsEmail.Body(cashierMsg);
           em.SendEmail(cashier.NotificationEmail, null, null, "" + Token.Mr + ": " + cashier.FullName, cashierFormalmessage, true);


       }

       public void AccountantWaitingApprovalNotify(string userId, string requestSerial)
       {
           var user = uow.UserRepository.FindById(userId);
           var accountants = uow.UserRepository.Get(a => a.BranchId == user.BranchId && a.Roles.Any(r => r.Id == "8514cab7-6465-49f0-8ebb-280c27e0ab27")).ToList();
           foreach (var a in accountants)
           {
               // send message to cashier
               var msg =
                  "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                  Token.YouHaveWaitingApprovalPettyCashRequestFrom + "</span></div>" + user.FullName +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
                  "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.PettyCashRequestSerialEmail +
                  "</div> <span> <br/>" + requestSerial +
                  "</h3></div>";

               var formalmessage = ClsEmail.Body(msg);
               em.SendEmail(a.NotificationEmail, null, null, "" + Token.Mr + ": " + a.FullName, formalmessage, true);



           }
       }
       public void AuthWaitingApprovalNotify(string userId, string requestSerial)
       {
           var user = uow.UserRepository.FindById(userId);
           var auths = uow.UserRepository.Get(a => a.BranchId == user.BranchId && a.Roles.Any(r => r.Id == "e38eeb34-7c32-43c7-9a1d-36734471f8dd")).ToList();
           foreach (var a in auths)
           {
               // send message to cashier
               var msg =
                  "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                  Token.YouHaveWaitingApprovalPettyCashRequestFrom + "</span></div>" + user.FullName +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
                  "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.PettyCashRequestSerialEmail +
                  "</div> <span> <br/>" + requestSerial +
                  "</h3></div>";

               var formalmessage = ClsEmail.Body(msg);
               em.SendEmail(a.NotificationEmail, null, null, "" + Token.Mr + ": " + a.FullName, formalmessage, true);



           }
       }
       public void CashierApprovedNotify(string userId, string requestSerial, string cashierId)
       {
           //var user = uow.UserRepository.FindById(userId);
           var c = uow.UserRepository.FindById(cashierId );
          
               // send message to cashier
               var msg =
                  "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                  Token.PettyCashRequestHasBeenApproved + "</span></div>" +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
                  "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.PettyCashRequestSerialEmail +
                  "</div> <span> <br/>" + requestSerial +
                  "</h3></div>";

               var formalmessage = ClsEmail.Body(msg);
               em.SendEmail(c.NotificationEmail, null, null, "" + Token.Mr + ": " + c.FullName, formalmessage, true);
       }
       public void UserReceivedNotify(string userId, string requestSerial, string requestUserId)
       {
           //var user = uow.UserRepository.FindById(userId);
           var c = uow.UserRepository.FindById(requestUserId);

           // send message to cashier
           var msg =
              "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
              Token.PettyCashRequestHasBeenReceived + "</span></div>" +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
              "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.PettyCashRequestSerialEmail +
              "</div> <span> <br/>" + requestSerial +
              "</h3></div>";

           var formalmessage = ClsEmail.Body(msg);
           em.SendEmail(c.NotificationEmail, null, null, "" + Token.Mr + ": " + c.FullName, formalmessage, true);
       }

       public void AddReimbursementNotify(string userId, string requestSerial)
       {

           var user = uow.UserRepository.FindById(userId);
           var cashier = uow.UserRepository.Get(a => a.BranchId == user.BranchId && a.Roles.Any(r => r.Id == "deb8cf0f-2f8f-4dfa-b8b9-ca0be155e238")).FirstOrDefault();

           // send message to user
           var userMsg =
               "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
               Token.YouSendReimbursementRequestTo + "</span></div>" + cashier.FullName +
               "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
               "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
               "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.ReimRequestSerialEmail +
               "</div> <span> <br/>" + requestSerial +
               "</h3></div>";


           var formalmessage = ClsEmail.Body(userMsg);
           em.SendEmail(user.NotificationEmail, null, null, "" + Token.User + ": " + user.FullName, formalmessage, true);

           // send message to cashier
           var cashierMsg =
              "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
              Token.YouHaveWaitingApprovalReimRequestFrom + "</span></div>" + user.FullName +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
              "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.ReimRequestSerialEmail +
              "</div> <span> <br/>" + requestSerial +
              "</h3></div>";

           var cashierFormalmessage = ClsEmail.Body(cashierMsg);
           em.SendEmail(cashier.NotificationEmail, null, null, "" + Token.Mr + ": " + cashier.FullName, cashierFormalmessage, true);


       }

       public void Accountant_WaitingApproval_Reimbursement_Notify(string userId, string requestSerial)
       {
           var user = uow.UserRepository.FindById(userId);
           var accountants = uow.UserRepository.Get(a => a.BranchId == user.BranchId && a.Roles.Any(r => r.Id == "8514cab7-6465-49f0-8ebb-280c27e0ab27")).ToList();
           foreach (var a in accountants)
           {
               // send message to cashier
               var msg =
                  "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                  Token.YouHaveWaitingApprovalReimRequestFrom + "</span></div>" + user.FullName +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
                  "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + Token.ReimRequestSerialEmail +
                  "</div> <span> <br/>" + requestSerial +
                  "</h3></div>";

               var formalmessage = ClsEmail.Body(msg);
               em.SendEmail(a.NotificationEmail, null, null, "" + Token.Mr + ": " + a.FullName, formalmessage, true);



           }
       }
       public void Auth_WaitingApproval_Reimbursement_Notify(string userId, string requestSerial)
       {
           var user = uow.UserRepository.FindById(userId);
           var auths = uow.UserRepository.Get(a => a.BranchId == user.BranchId && a.Roles.Any(r => r.Id == "e38eeb34-7c32-43c7-9a1d-36734471f8dd")).ToList();
           foreach (var a in auths)
           {
               // send message to cashier
               var msg =
                  "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
                  Token.YouHaveWaitingApprovalReimRequestFrom + "</span></div>" + user.FullName +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
                  "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
                  "</h3><h3 ><div style='font-weight: bold; color: blue; '>"+ Token.ReimRequestSerialEmail +
                  "</div> <span> <br/>" + requestSerial +
                  "</h3></div>";

               var formalmessage = ClsEmail.Body(msg);
               em.SendEmail(a.NotificationEmail, null, null, "" + Token.Mr + ": " + a.FullName, formalmessage, true);

           }
       }
       public void Cashier_Approved_Reimbursement_Notify(string userId, string requestSerial, string cashierId)
       {
           //var user = uow.UserRepository.FindById(userId);
           var c = uow.UserRepository.FindById(cashierId);

           // send message to cashier
           var msg =
              "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
              Token.YouHaveWaitingApprovalReimRequestFrom + "</span></div>" +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
              "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>"  + Token.ReimRequestSerialEmail +
              "</div> <span> <br/>" + requestSerial +
              "</h3></div>";

           var formalmessage = ClsEmail.Body(msg);
           em.SendEmail(c.NotificationEmail, null, null, "" + Token.Mr + ": " + c.FullName, formalmessage, true);
       }
       public void User_Received_Reimbursement_Notify(string userId, string requestSerial, string requestUserId)
       {
           //var user = uow.UserRepository.FindById(userId);
           var c = uow.UserRepository.FindById(requestUserId);

           // send message to cashier
           var msg =
              "  <div style='margin: 20px auto;width: 80%;text-align:center;'><h3 style='font-weight: bold;color:#666;'><div style='dir:rtl;'><span style='font-weight: bold; color: blue; '>" +
              Token.ReimbursementRequestHasBeenReceived + "</span></div>" +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>" + " : " + Token.Date +
              "</div> <span> <br/>" + DateTime.Now.GetCurrentTime(userId).ToString("dd/MM/yyyy") +
              "</h3><h3 ><div style='font-weight: bold; color: blue; '>"  + Token.ReimRequestSerialEmail +
              "</div> <span> <br/>" + requestSerial +
              "</h3></div>";

           var formalmessage = ClsEmail.Body(msg);
           em.SendEmail(c.NotificationEmail, null, null, "" + Token.Mr + ": " + c.FullName, formalmessage, true);
       }

    }
}
