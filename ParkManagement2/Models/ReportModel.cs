using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkManagement2.Models
{
    public class ReportModel
    {
        public string CarId { get; set; }
        public string Tag { get; set; }
        public double TotalPaied { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}