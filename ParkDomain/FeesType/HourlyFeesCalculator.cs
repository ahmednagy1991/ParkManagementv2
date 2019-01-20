using ParkDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkDomain.FeesType
{
    public class HourlyFeesCalculator : IParkFeesCalculator
    {
        public int FeesTypes
        {
            get
            {
                return 1;
            }
        }

        
        public double GetFees(DateTime TimeIn, DateTime TimeOut, DateTime Hotelout, bool check, double FeesPerPeriod, double GracePeriod)
        {

            if (check)
            {
                return 0;
            }
            TimeSpan span = TimeOut - TimeIn;
            var hours = (float)span.Hours;
            var mins = (float)span.Minutes;
            mins += hours * 60;
            mins += span.Days * 1440;
            hours = (float)Math.Ceiling((decimal)(mins / 60));
            return hours * FeesPerPeriod;
        }
    }
}
