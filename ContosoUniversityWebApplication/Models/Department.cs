using ContosoUniversityWebApplication.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversityWebApplication.Models
{
    [Author("Andy", version=1.0)]
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        /*
         *A question mark is added after the int type designation to mark the property as nullable.
         * By convention, the Entity Framework enables cascade delete for non-nullable foreign keys and for many-to-many relationships. 
         * This can result in circular cascade delete rules, which will cause an exception when you try to add a migration.
         */
         [Display(Name = "Administrator")]
        public int? InstructorID { get; set; }

        //Tracking property for the Optimistic Concurrency
        [Timestamp]
        //The Timestamp attribute specifies that this column will be included in the Where clause of Update and Delete commands sent to the database.
        //A row version type (also known as a sequence number) is a binary number that is guaranteed to be unique in the database. It does not represent an actual time. 
        //Row version data is not visually meaningful. 
        //Therefore, when the TimestampAttribute attribute is used with a Dynamic Data field, the column is not displayed unless the ScaffoldColumnAttribute attribute of the column is explicitly set to true.
        public byte[] RowVersion { get; set; }

        public virtual Instructor Administrator { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

    }
}