//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class qryLatestStudentGrade
    {
        public Nullable<long> Row { get; set; }
        public Nullable<long> Rank { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public Nullable<double> Grade { get; set; }
        public Nullable<System.DateTime> DateAchieved { get; set; }
        public int CourseWorkID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleShortName { get; set; }
        public string CourseWorkTitle { get; set; }
        public string Mark { get; set; }
    }
}
