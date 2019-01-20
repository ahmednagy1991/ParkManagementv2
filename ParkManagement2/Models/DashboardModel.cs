using ParkManagement2.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkManagement.Models
{
    public class DashboardModel
    {
        public int AviParks { get; set; }
        public int VacParks { get; set; }
        public int TodayParks { get; set; }
        public int AllParks { get; set; }
        public List<Park> parks { get; set; }
    }
}