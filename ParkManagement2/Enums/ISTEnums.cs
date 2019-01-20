using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkManagement.Enums
{
    public class ISTEnums
    {
        public enum ParkMode
        {
            In=1,
            Out=2
        }
        public enum FeesMode
        {
            Min = 1,
            Days = 2,
            Free=3
        }
        public enum CarStatus
        {
            Park=1,
            AtDoor=2,
            Out=3
        }
    }
}