using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {


        private EmailSettings emailSettings;
        private EFDbContext context = new EFDbContext();


        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public IEnumerable<Order> Orders
        {
            get { return context.Orders; }
        }
       
        public Order DeleteOrder(int orderID)
        {
            Order dbEntry = context.Orders.Find(orderID);
            if (dbEntry != null)
            {
                context.Orders.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                = new NetworkCredential(emailSettings.Username,
                emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                    = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("---")
                .AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity,
                    line.Product.Name,
                    subtotal);
                }
                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Ship to:")
                .AppendLine(shippingInfo.Name);
                
                MailMessage mailMessage = new MailMessage(
                emailSettings.MailFromAddress, // From
                emailSettings.MailToAddress, // To
                "New order submitted!", // Subject
                body.ToString()); // Body
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }

        public void SaveOrder(List<Order> orders)
        {
           foreach(var o in orders)
            {
                context.Orders.Add(o);
            }
            context.SaveChanges();
        }
    }
}
