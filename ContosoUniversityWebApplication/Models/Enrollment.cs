using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityWebApplication.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        //An enrollment record is for a single course, so there's a CourseID foreign key property and a Course navigation property.
        public int CourseID { get; set; }
        //An enrollment record is for a single student, so there's a StudentID foreign key property and a Student navigation property.
        public int StudentID { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; } //
         
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}