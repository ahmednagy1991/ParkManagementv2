using ParkManagement2.Database;
using ParkManagement2.fitlers;
using ParkManagement2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkManagement2.Controllers
{
    public class SmsTemplateController : Controller
    {
        ISTParkManagementEntities DB;
        public SmsTemplateController()
        {
            DB = new ISTParkManagementEntities();

        }

        [CustomAuthentication]
        // GET: SmsTemplate
        public ActionResult Index()
        {
            var model = new SmsModel();
            model.templates = DB.sms_template.ToList();
            return View(model);
        }

        [CustomAuthentication]
        public ActionResult AddNew(SmsModel model)
        {
            if (model == null)
            {
                model = new SmsModel();
                model.template = new sms_template();
            }
            return View(model);
        }

        [CustomAuthentication]
        public ActionResult Edit(int id)
        {
            var model = new SmsModel();
            model.template = DB.sms_template.Where(m => m.id == id).FirstOrDefault();
            return View("AddNew", model);
        }

        [CustomAuthentication]
        public ActionResult Save(SmsModel model)
        {
            if(model.template.id>0)
            {
                var template = DB.sms_template.Where(m => m.id == model.template.id).FirstOrDefault();
                DB.sms_template.Attach(template);
                template.template_name = model.template.template_name;
                template.tamplate_body = model.template.tamplate_body;
                DB.SaveChanges();
            }
            else
            {
                DB.sms_template.Add(model.template);
                DB.SaveChanges();

            }
          

            return RedirectToAction("Index");
        }
    }
}