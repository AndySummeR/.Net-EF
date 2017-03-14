using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversityWebApplication.Models
{
    public class Instructor: Person
    {

        [DataType(DataType.Date), Display(Name = "Hire Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        //The Courses and OfficeAssignment properties are navigation properties
        //they are typically defined as virtual so that they can take advantage of an Entity Framework feature called lazy loading. 
        //In addition, if a navigation property can hold multiple entities, its type must implement the ICollection<T> Interface. 
        //For example IList<T> qualifies but not IEnumerable<T> because IEnumerable<T> doesn't implement Add.
        public virtual ICollection<Course> Courses { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }

    }
}
