using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkDomain.Interface
{
    public interface IParkFeesCalculator
    {
        int FeesTypes { get; }       
        double GetFees(DateTime TimeIn, DateTime TimeOut, DateTime Hotelout, bool check, double FeesPerPeriod,double GracePeriod);
    }
}
