using ParkDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkDomain.FeesType
{
    public class PaiedByHotelFeesCalculator : IParkFeesCalculator
    {
        //public double FeesPerPeriod { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int FeesTypes
        {
            get
            {
                return 3;
            }
        }

      

        public double GetFees(DateTime TimeIn, DateTime TimeOut, DateTime Hotelout, bool check, double FeesPerPeriod, double GracePeriod)
        {
            TimeSpan span = TimeOut - Hotelout;
            if (span.Minutes <= GracePeriod)
            {
                return 0;
            }

            var hours = (double)span.Hours;
            var mins = (double)span.Minutes;
            mins -= GracePeriod;
            mins += hours * 60;
            mins += span.Days * 1440;
            hours = (float)Math.Ceiling((decimal)(mins / 60));
            return hours * FeesPerPeriod;
        }
    }
}
