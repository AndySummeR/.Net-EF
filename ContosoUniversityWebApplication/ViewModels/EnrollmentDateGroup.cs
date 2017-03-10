using System;
using System.ComponentModel.DataAnnotations;
using ContosoUniversityWebApplication.Models;


namespace ContosoUniversityWebApplication.ViewModels
{
    public class EnrollmentDateGroup
    {

        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }

    }
}