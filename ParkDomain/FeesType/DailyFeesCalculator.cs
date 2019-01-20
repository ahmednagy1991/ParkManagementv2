using ParkDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkDomain.FeesType
{
    public class DailyFeesCalculator : IParkFeesCalculator
    {
        public int FeesTypes
        {
            get
            {
                return 2;
            }
        }

        //public double FeesPerPeriod { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double GetFees(DateTime TimeIn, DateTime TimeOut, DateTime Hotelout, bool check, double FeesPerPeriod, double GracePeriod)
        {
            TimeSpan span = TimeOut - TimeIn;
            var hours = span.Hours;
            var days = (float)span.Days;
            days += hours * 24;
            days = (float)Math.Ceiling((decimal)(hours / 24));
            if (days == 0 && span.Minutes > 0)
            {
                days++;
            }

            return days * FeesPerPeriod;
        }
    }
}
