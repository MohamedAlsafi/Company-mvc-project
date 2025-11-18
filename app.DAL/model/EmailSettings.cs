using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.model
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSSL { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
