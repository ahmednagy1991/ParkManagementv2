using ParkManagement2.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkManagement2.Models
{
    public class SmsModel
    {
        public List<sms_template> templates { get; set; }
        public sms_template template { get; set; }
    }
}