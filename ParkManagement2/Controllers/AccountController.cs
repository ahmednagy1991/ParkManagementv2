using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.Owin.Security;
using ParkManagement.Models;
using ParkManagement2.Authentication;
using ParkManagement2.Database;

namespace ParkManagement.Controllers
{

    public class AccountController : Controller
    {
        ISTParkManagementEntities DB = new ISTParkManagementEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register(User user_model)
        {
            if (user_model == null)
            {
                user_model = new User();
            }
            return View(user_model);
        }


        public ActionResult Login()
        {
            var user_model = new User();
            return View(user_model);
        }

        public ActionResult Logout()
        {
            Response.Cookies.Add(TokenManager.Logout());
            return View("Login");
        }



        public ActionResult ValidateUser(User user_model)
        {
            if (ModelState.IsValid)
            {

                //var user = userService.Login(user_model.Username, PasswordHash.HashPassword(user_model.Password));
                var user = DB.Users.Where(m => m.Username == user_model.Username && m.Password == user_model.Password).FirstOrDefault();
                if (user != null)
                {
                    var token = TokenManager.GenerateToken(user.Username, user.UserId.ToString());
                    Response.Cookies.Add(TokenManager.CreateCookie(token));
                    HttpContext.User = TokenManager.GetPrincipal(token);
                    return RedirectToAction("Index", "Home");
                }
                TempData["error"] = "Error in username or password";
                return RedirectToAction("Login");
            }
            TempData["error"] = String.Join(" & ", ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage).ToArray());
            return RedirectToAction("Login");
        }



    

    }
}