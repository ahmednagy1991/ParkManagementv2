using ParkManagement.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ParkManagement
{
    public class Business
    {
        public double GetFees(DateTime TimeIn, DateTime TimeOut, DateTime Hotelout, int FeeMode,bool check)
        {
            if (FeeMode == (int)ISTEnums.FeesMode.Days)
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
                return days * float.Parse(ConfigurationManager.AppSettings["Dayfess"]);
            }
            else if (FeeMode == (int)ISTEnums.FeesMode.Min)
            {
                //TimeSpan Hspan = TimeOut - Hotelout;
                //if (Hspan.Minutes <= int.Parse(ConfigurationManager.AppSettings["Graceperiod"]))
                //{
                //    return 0;
                //}

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
                return hours * float.Parse(System.Configuration.ConfigurationManager.AppSettings["HourFees"]);

            }
            else if (FeeMode == (int)ISTEnums.FeesMode.Free)
            {
                TimeSpan span = TimeOut - Hotelout;
                if(span.Minutes<=int.Parse(ConfigurationManager.AppSettings["Graceperiod"]))
                {
                    return 0;
                }
                
                var hours = (float)span.Hours;
                var mins = (float)span.Minutes;
                mins -= int.Parse(ConfigurationManager.AppSettings["Graceperiod"]);
                mins += hours * 60;
                mins += span.Days * 1440;
                hours = (float)Math.Ceiling((decimal)(mins / 60));             
                return hours * float.Parse(System.Configuration.ConfigurationManager.AppSettings["HourFees"]);
            }

            return 0;
        }
    }
}