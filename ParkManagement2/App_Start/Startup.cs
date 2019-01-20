using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Routing;


[assembly: OwinStartup(typeof(ParkManagement2.App_Start.Startup))]
namespace ParkManagement2.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
            app.MapSignalR("/signalr", new HubConfiguration());
           
        }

       
    }
}