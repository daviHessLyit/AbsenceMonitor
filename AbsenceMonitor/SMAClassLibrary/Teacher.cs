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
    
    public partial class Teacher
    {
        public int TeacherId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public int ClassId { get; set; }

        public string FullName
        {
            get
            {
                return $"{GivenName} {Surname}";
            }
        }

        public virtual Class Class { get; set; }
    }
}
