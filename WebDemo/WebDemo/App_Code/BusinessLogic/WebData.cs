using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using WebDemo;
using WebDemo.Models;

namespace WebDemo.App_Code.BusinessLogic
{
    public class WebData
    {

        public StudentModel GetStudentModel(string studentRowGuid)
        {
            StudentModel result = new StudentModel();
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                result.StudentInfo = GetStudentData(studentRowGuid);
                result.StudentInfo.Password = null;
                if (result.StudentInfo != null)
                {
                    result.StudentGrades = GetStudentGrades(result.StudentInfo.ID);
                }
                result.StudentModules = GetStudentModules(result.StudentInfo.CourseID.ToString());
                int[] moduleIDs = result.StudentModules.Select(y => (int)y.ModuleID).ToArray();
                result.ModuleCourseWork = GetStudentCourseWork(moduleIDs);
                result.LatestStudentGrades = GetLatestStudentGrades(result.StudentInfo.ID);
                result.CourseInfo = GetCourseInfo(result.StudentInfo.CourseID);
            }
            return result;
        }

        public tblCours GetCourseInfo(int courseID)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                return ent.tblCourses.Where(x => x.CourseID == courseID).FirstOrDefault();
            }
        }

        public qryLatestStudentGrade[] GetLatestStudentGrades(int? studentID)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                return ent.qryLatestStudentGrades.Where(x => x.StudentID == studentID).ToArray();
            }
        }

        public tblCourseWork[] GetStudentCourseWork(int[] modules)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                return ent.tblCourseWorks.Where(x => modules.Contains((int)x.ModuleID)).ToArray();
            }
        }

        public tblModule[] GetStudentModules(string courseID)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                return ent.tblModules.Where(a => a.CourseID == courseID).ToArray();
            }
        }

        public qryStudentGrade[] GetStudentGrades(int? studentID)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                return ent.qryStudentGrades.AsNoTracking().Where(a => a.StudentID == studentID).ToArray();
            }
        }

        public tblStudent GetStudentData(string rowGuid)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                return ent.tblStudents.Where(a => a.RowGuid.ToString() == rowGuid).FirstOrDefault();
            }
        }

        public StudentModel SaveStudentDetails (StudentModel model, string studentRowGuid)
        {
            using (leonardc_WebDemoEntities1 ent = new leonardc_WebDemoEntities1())
            {
                ent.tblStudents.Where(x => x.RowGuid.ToString() == studentRowGuid).FirstOrDefault().Password = model.StudentInfo.Password;
                ent.SaveChanges();
            }
            return GetStudentModel(studentRowGuid);
        }
    }
}