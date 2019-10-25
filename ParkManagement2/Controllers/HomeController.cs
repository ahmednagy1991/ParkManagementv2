using Microsoft.AspNet.SignalR;
using ParkDomain;
using ParkDomain.FeesType;
using ParkManagement2.Database;
using ParkManagement.Enums;
using ParkManagement.Hubs;
using ParkManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParkManagement2.Models;
using ParkManagement2.fitlers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using ParkManagement2.Helpers;

namespace ParkManagement.Controllers
{
    public class HomeController : Controller
    {
        ISTParkManagementEntities DB;
        ParkService Park_service;
        public HomeController()
        {
            //Park_service = new ParkService(new DailyFeesCalculator());
            // Park_service = new ParkService(new FeesFactory().CreateInstance(int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"])));
            //if(int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"])==1)
            //{
            //    Park_service = new HourlyFeesCalculator();
            //}

            Park_service = new ParkService(new FeesFactory().CreateInstance(int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"])));
            DB = new ISTParkManagementEntities();
        }
        public ActionResult Chat()
        {

            return View();
        }

        [CustomAuthentication]
        public ActionResult DailyReport(DateTime From, DateTime TO)
        {
            //Park_service.CalculateFees(park.TimeIn ?? DateTime.Now, park.TimeOut ?? DateTime.Now, park.HotelOutTime ?? DateTime.Now, park.Checked ?? false, double.Parse(System.Configuration.ConfigurationManager.AppSettings["HourFees"]), double.Parse(System.Configuration.ConfigurationManager.AppSettings["Graceperiod"]));
            var parks = DB.Parks.Where(m => m.TimeIn >= From && m.TimeIn <= TO).Select(m => new ReportModel { Tag = m.Tag, From = m.TimeIn.ToString(), To = m.TimeOut.ToString(), CarId = m.CarId, TotalPaied = m.Fees ?? 0 }).ToList();
            //var parks = DB.Parks.Where(m => m.TimeIn >= From && m.TimeOut <= TO).ToList();
            return View(parks);
        }

        [CustomAuthentication]
        public ActionResult ViewDailyReport()
        {
            return View();
        }

        [CustomAuthentication]
        public ActionResult Index()
        {
            //var model = new DashboardModel();
            ////var temp = DB.Parks;             
            //model.AviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - DB.Parks.Where(m => m.TimeOut == null).Count());
            //model.VacParks = DB.Parks.Where(m => m.TimeOut == null).Count();
            //return View(model);          
            var model = new DashboardModel();
            var Parks = DB.Parks.OrderByDescending(m => m.Id).ToList();
            model.parks = Parks.Where(m => m.CarStatus == (int)ISTEnums.CarStatus.Park || m.CarStatus == (int)ISTEnums.CarStatus.AtDoor || m.CarStatus == null).ToList();
            model.AviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - Parks.Where(m => m.TimeOut == null).Count());
            model.VacParks = Parks.Where(m => m.TimeOut == null).Count();
            model.AllParks = Parks.Count();
            model.message_templates = DB.sms_template.Select(m => new SelectListItem() { Text = m.template_name, Value = m.id.ToString() }).ToList();
            model.TodayParks = Parks.Where(m => m.TimeIn == DateTime.Today).Count();
            return View(model);

        }

        public ActionResult LoadParks()
        {
            var model = new DashboardModel
            {
                //|| m.CarStatus == null
                parks = DB.Parks.Where(m => m.CarStatus == (int)ISTEnums.CarStatus.Park || m.CarStatus == (int)ISTEnums.CarStatus.AtDoor ).OrderByDescending(m => m.Id).ToList(),
                message_templates = DB.sms_template.Select(m => new SelectListItem() { Text = m.template_name, Value = m.id.ToString() }).ToList()
            };
            return PartialView(model);
        }


        public JsonResult AtDoor(long ParkId)
        {
            var Parks = DB.Parks.Where(m => m.Id == ParkId).FirstOrDefault();
            DB.Parks.Attach(Parks);
            Parks.CarStatus = (int)ISTEnums.CarStatus.AtDoor;
            DB.SaveChanges();
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Out(long ParkId)
        {
            var Parks = DB.Parks.Where(m => m.Id == ParkId).FirstOrDefault();
            DB.Parks.Attach(Parks);
            Parks.CarStatus = (int)ISTEnums.CarStatus.Out;
            DB.SaveChanges();
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.Admit("ahmed", "hi");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [CustomAuthentication]
        public ActionResult GenerateQR()
        {
            return View();
        }

        public ActionResult ViewQR(string qr)
        {
            return PartialView("~/Views/Home/ViewQR.cshtml", qr);
        }


        public JsonResult GetPritnerMAC()
        {
            return Json(System.Configuration.ConfigurationManager.AppSettings["PrinterMACAddress"], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPHoneNumber()
        {
            return Json(System.Configuration.ConfigurationManager.AppSettings["Phone"], JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAppName()
        {
            return Json(System.Configuration.ConfigurationManager.AppSettings["AppName"], JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFees()
        {
            return Json(System.Configuration.ConfigurationManager.AppSettings["HourFeesString"], JsonRequestBehavior.AllowGet);
        }



        public JsonResult Leave(string Tag)
        {
            var AllPark = DB.Parks.ToList();
            var park = AllPark.Where(m => m.Tag == Tag && m.TimeOut == null).FirstOrDefault();
            DB.Parks.Attach(park);
            park.TempLeave = true;
            DB.SaveChanges();
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckLeave(string Tag)
        {
            var AllPark = DB.Parks.ToList();
            var park = AllPark.Where(m => m.Tag == Tag && m.TimeOut == null).FirstOrDefault();
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            if (park != null && park.TempLeave.Value)
            {
                context.Clients.All.CheckLeave("Client can go for free");
            }
            else
            {
                context.Clients.All.CantLeave("This client can not leave please check hotel");
            }
            return Json("Done", JsonRequestBehavior.AllowGet);
        }


        public JsonResult Admit(string CarId, string Tag, string draw, int mode)
        {

            //var AllPark = DB.Parks;
            //var park = AllPark.Where(m => m.Tag == Tag && m.TimeOut == null).FirstOrDefault();
            //var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            //if (park != null)
            //{
            //    context.Clients.All.AlreadyExisits("Tag Already used");
            //}
            //else
            //{
            //    DB.Parks.Add(new Park() { CarId = CarId, DrawId = draw, FeesMode = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"]), Tag = Tag, TimeIn = DateTime.Now });
            //    DB.SaveChanges();

            //    var aviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - DB.Parks.Where(m => m.TimeOut == null).Count());
            //    var vacParks = DB.Parks.Where(m => m.TimeOut == null).Count();


            //    var AllParks = AllPark.Count();
            //    var TodayParks = AllPark.Where(m => m.TimeIn == DateTime.Today).Count();
            //    context.Clients.All.Admit(CarId, Tag, DateTime.Now, "In", aviParks, vacParks);
            //}

            var AllPark = DB.Parks.ToList();
            var park = AllPark.Where(m => m.Tag == Tag && m.TimeOut == null).FirstOrDefault();
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            if (park != null)
            {
                context.Clients.All.AlreadyExisits("Tag Already used");
                var model = new AdmitModel();
                model.Message = "Err";
                model.ToastMessage = "Tag already in use";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //CarStatus = (int)ISTEnums.CarStatus.Park
                var parObj = new Park() { CarId = CarId, DrawId = draw, FeesMode = int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"]), Tag = Tag, TimeIn = DateTime.Now };
                DB.Parks.Add(parObj);
                DB.SaveChanges();
                AllPark = DB.Parks.ToList();
                var aviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - AllPark.Where(m => m.TimeOut == null).Count());
                var vacParks = AllPark.Where(m => m.TimeOut == null).Count();
                var AllParks = AllPark.Count();
                var TodayParks = AllPark.Where(m => m.TimeIn == DateTime.Today).Count();
                context.Clients.All.Admit(CarId, Tag, DateTime.Now, "In", aviParks, vacParks, AllParks, TodayParks);
                var model = new AdmitModel();
                model.Message = "Done";
                model.AdmitDate = parObj.TimeIn.Value.ToString();
                return Json(model, JsonRequestBehavior.AllowGet);
            }


        }


        public JsonResult HotelOut(string CarId, string Tag, string draw, int mode)
        {

            //var park = DB.Parks.Where(m => m.Tag == Tag && m.TimeOut == null).FirstOrDefault();
            var park = DB.Parks.Where(m => m.Tag == Tag).FirstOrDefault();
            DB.Parks.Attach(park);
            park.HotelOutTime = DateTime.Now;
            park.Checked = true;
            park.TimeOut = DateTime.Now;
            park.TempLeave = false;
            park.Fees = 0;
            park.CarStatus = (int)ISTEnums.CarStatus.Park;
            DB.SaveChanges();
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.CheckLeave("Client can go for free");

            var AllPark = DB.Parks.ToList();
            AllPark = DB.Parks.ToList();

            var aviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - AllPark.Where(m => m.TimeOut == null).Count());
            var vacParks = AllPark.Where(m => m.TimeOut == null).Count();

            var AllParks = AllPark.Count();
            var TodayParks = AllPark.Where(m => m.TimeIn == DateTime.Today).Count();


            //HotelPickup
            //context.Clients.All.Pickup(park.CarId, park.Tag, DateTime.Now, "out", aviParks, vacParks, 0, park.HotelOutTime, AllParks, TodayParks, park.Id);
            context.Clients.All.HotelPickup(park.CarId, park.Tag, DateTime.Now, "out", aviParks, vacParks, 0, park.HotelOutTime, AllParks, TodayParks, park.Id);
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Pickup(string CarId, string Tag, string draw, int mode)
        {
            Business fees = new Business();
            var AllPark = DB.Parks.ToList();
            var park = AllPark.Where(m => m.Tag == Tag && m.TimeOut == null).FirstOrDefault();
            DB.Parks.Attach(park);
            park.TimeOut = DateTime.Now;
            park.CarStatus = (int)ISTEnums.CarStatus.Park;
            var val = fees.GetFees(park.TimeIn ?? DateTime.Now, DateTime.Now, park.HotelOutTime ?? DateTime.Now, int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"]), park.Checked ?? false);

            //var val = Park_service.CalculateFees(park.TimeIn ?? DateTime.Now, park.TimeOut ?? DateTime.Now, park.HotelOutTime ?? DateTime.Now, park.Checked ?? false, double.Parse(System.Configuration.ConfigurationManager.AppSettings["HourFees"]), double.Parse(System.Configuration.ConfigurationManager.AppSettings["Graceperiod"]));
            park.Fees = val;
            DB.SaveChanges();
            AllPark = DB.Parks.ToList();

            var aviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - AllPark.Where(m => m.TimeOut == null).Count());
            var vacParks = AllPark.Where(m => m.TimeOut == null).Count();

            var AllParks = AllPark.Count();
            var TodayParks = AllPark.Where(m => m.TimeIn == DateTime.Today).Count();


            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();



            context.Clients.All.Pickup(CarId, Tag, DateTime.Now, "out", aviParks, vacParks, val, park.HotelOutTime, AllParks, TodayParks, park.Id);
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendMessage(int TemplateId, int ParkId)
        {
            var message = DB.sms_template.Where(m => m.id == TemplateId).FirstOrDefault().tamplate_body;
            var mobile = DB.Parks.Where(m => m.Id == ParkId).FirstOrDefault().from_mobile_number;
            SMSHelper.SendMessage(message, mobile);
            return Json("Message Sent", JsonRequestBehavior.AllowGet);
        }

        public JsonResult PickupBySMS(string Message, string mobile_number)
        {
            Business fees = new Business();
            var AllPark = DB.Parks.ToList();
            var park = AllPark.Where(m => m.Tag == Message && m.TimeOut == null).FirstOrDefault();
            if (park != null)
            {

                DB.Parks.Attach(park);
                park.TimeOut = DateTime.Now;
                park.From_sms = true;
                park.from_mobile_number = mobile_number;
                park.CarStatus = (int)ISTEnums.CarStatus.Park;
                var val = fees.GetFees(park.TimeIn ?? DateTime.Now, DateTime.Now, park.HotelOutTime ?? DateTime.Now, int.Parse(System.Configuration.ConfigurationManager.AppSettings["FeesMode"]), park.Checked ?? false);
                park.Fees = val;
                DB.SaveChanges();
                var aviParks = (int.Parse(System.Configuration.ConfigurationManager.AppSettings["TotalParks"]) - AllPark.Where(m => m.TimeOut == null).Count());
                var vacParks = AllPark.Where(m => m.TimeOut == null).Count();

                var AllParks = AllPark.Count();
                var TodayParks = AllPark.Where(m => m.TimeIn == DateTime.Today).Count();


                var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                SMSHelper.SendMessage(DB.sms_template.Where(m => m.template_name == "Pickup").FirstOrDefault().tamplate_body, mobile_number);

                context.Clients.All.PickupBySMS(park.CarId, park.Tag, DateTime.Now, "out", aviParks, vacParks, val, park.HotelOutTime, AllParks, TodayParks, park.Id);
            }
            return Json("Done", JsonRequestBehavior.AllowGet);
        }



    }
}