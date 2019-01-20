using ParkDomain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkDomain
{
    public class ParkService
    {
        IParkFeesCalculator FeesCalculator;
        public ParkService(IParkFeesCalculator FeesCalculator)
        {
            this.FeesCalculator = FeesCalculator;
        }
        public double CalculateFees(DateTime TimeIn, DateTime TimeOut, DateTime Hotelout, bool check, double FeesPerPeriod, double GracePeriod)
        {
            return FeesCalculator.GetFees(TimeIn, TimeOut, Hotelout, check, FeesPerPeriod, GracePeriod);
        }
    }
}
