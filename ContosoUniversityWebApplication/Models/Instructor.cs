using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversityWebApplication.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        //You could remove the Required attribute and replace it with a minimum length parameter for the StringLength attribute:
        //[Required]
        [Display(Name = "Last Name"), StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        [Column("FirstName"), Display(Name = "First Name"), StringLength(50, MinimumLength = 1)]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date), Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        //The Courses and OfficeAssignment properties are navigation properties
        //they are typically defined as virtual so that they can take advantage of an Entity Framework feature called lazy loading. 
        //In addition, if a navigation property can hold multiple entities, its type must implement the ICollection<T> Interface. 
        //For example IList<T> qualifies but not IEnumerable<T> because IEnumerable<T> doesn't implement Add.
        public virtual ICollection<Course> Courses { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }

    }
}
