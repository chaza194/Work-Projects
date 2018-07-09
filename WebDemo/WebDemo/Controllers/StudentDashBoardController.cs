using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using WebDemo.Models;
using WebDemo.App_Code.Attributes;
using WebDemo.App_Code.BusinessLogic;

namespace WebDemo.Controllers
{
    public class StudentDashBoardController : Controller
    {
        public WebData Data = new WebData();

        [NoCache]
        public ActionResult Index()
        {
            if (Session["StudentUniqueIdentifier"] == null)
            {
                return View("Login");
            }
            else
            {
                return View("DashBoard", Data.GetStudentModel(Session["StudentUniqueIdentifier"].ToString()));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
                {
                    tblStudent locStudent = ent.tblStudents.Where(a => a.UserName.Equals(student.StudentInfo.UserName) && a.Password.Equals(student.StudentInfo.Password)).FirstOrDefault();
                    if (locStudent != null)
                    {
                        Session["StudentUniqueIdentifier"] = locStudent.RowGuid;
                        return RedirectToAction("DashBoard", Data.GetStudentModel(locStudent.RowGuid.ToString()));
                    }
                }
            }
            return View();
        }
        [NoCache]
        public ActionResult Login()
        {
            if (Session["StudentUniqueIdentifier"] == null)
            {
                return View("Login");
            }
            else
            {
                return View("DashBoard", Data.GetStudentModel(Session["StudentUniqueIdentifier"].ToString()));
            }
        }
        [NoCache]
        public ActionResult DashBoard()
        {
            if (Session["StudentUniqueIdentifier"] == null)
            {
                return View("Login");
            }
            else
            {
                return View("DashBoard", Data.GetStudentModel(Session["StudentUniqueIdentifier"].ToString()));
            }
        }

        public ActionResult Logout()
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (Session["StudentUniqueIdentifier"] != null)
            {
                Session.Clear();
            }
            return View("Login");
        }

        [HttpPost]
        [NoCache]
        public ActionResult StudentDetails(StudentModel model)
        {
            if (Session["StudentUniqueIdentifier"] == null)
            {
                return View("Login");
            }
            else
            {
               StudentModel newMod = Data.SaveStudentDetails(model, Session["StudentUniqueIdentifier"].ToString());
               return View("StudentDetails", newMod);
            }
        }

        [NoCache]
        public ActionResult StudentDetails()
        {
            if (Session["StudentUniqueIdentifier"] == null)
            {
                return View("Login");
            }
            else
            {
                return View("StudentDetails", Data.GetStudentModel(Session["StudentUniqueIdentifier"].ToString()));
            }
        }
    }
}