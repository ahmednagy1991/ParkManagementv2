//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkManagement2.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Park
    {
        public long Id { get; set; }
        public string Tag { get; set; }
        public string CarId { get; set; }
        public string DrawId { get; set; }
        public Nullable<System.DateTime> TimeIn { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public Nullable<int> FeesMode { get; set; }
        public Nullable<bool> Checked { get; set; }
        public Nullable<System.DateTime> HotelOutTime { get; set; }
        public Nullable<bool> TempLeave { get; set; }
        public Nullable<int> CarStatus { get; set; }
        public Nullable<double> Fees { get; set; }
    }
}
