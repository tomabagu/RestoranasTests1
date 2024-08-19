using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restoranas.Interfaces;

namespace Restoranas.Services
{
    public class NoEmail : IEmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
        }
    }
}
