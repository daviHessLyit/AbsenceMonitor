﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SMADBEntities : DbContext
    {
        public SMADBEntities()
            : base("name=SMADBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Absence> Absences { get; set; }
        public virtual DbSet<AbsenceType> AbsenceTypes { get; set; }
        public virtual DbSet<AccessLevel> AccessLevels { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Guardian> Guardians { get; set; }
        public virtual DbSet<Pupil> Pupils { get; set; }
        public virtual DbSet<PupilAbsence> PupilAbsences { get; set; }
        public virtual DbSet<PupilAttendance> PupilAttendances { get; set; }
        public virtual DbSet<SystemEvent> SystemEvents { get; set; }
        public virtual DbSet<SystemUser> SystemUsers { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
    }
}
