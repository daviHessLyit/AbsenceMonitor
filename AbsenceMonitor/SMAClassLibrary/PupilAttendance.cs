//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMAClassLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class PupilAttendance
    {
        public int PupilAttendanceId { get; set; }
        public int PupilId { get; set; }
        public int AttendanceId { get; set; }
    
        public virtual Attendance Attendance { get; set; }
        public virtual Pupil Pupil { get; set; }
    }
}