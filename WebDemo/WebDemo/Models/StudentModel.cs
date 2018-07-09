using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebDemo.Models
{
    public class StudentModel
    {
        public tblStudent StudentInfo { get; set; }
        public tblModule[] StudentModules { get; set; }
        public tblCourseWork[] ModuleCourseWork { get; set; }
        public qryStudentGrade[] StudentGrades { get; set; }
        public qryLatestStudentGrade[] LatestStudentGrades { get; set; }
        public tblCours CourseInfo { get; set; }

        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime CurrentDateTime { get { return DateTime.Now; } }
    }
}