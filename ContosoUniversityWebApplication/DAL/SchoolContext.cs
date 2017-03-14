using ContosoUniversityWebApplication.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;



namespace ContosoUniversityWebApplication.DAL
{

    //This code creates a DbSet property for each entity set. In Entity Framework terminology, an entity set typically corresponds to a database table, and an entity corresponds to a row in the table.
    public class SchoolContext : DbContext
    {
        //public SchoolContext() : base("SchoolContext")   //The name of the connection string (which you will add to the Web.config file later) is passed in to the constructor
        //{
        //}
        //public DbSet<Student> Students { get; set; }
        //public DbSet<Enrollment> Enrollments { get; set; }
        //public DbSet<Course> Courses { get; set; }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        /*
         The new statement in the OnModelCreating method configures the many-to-many join table:
          For the many-to-many relationship between the Instructor and Course entities, the code specifies the table and column names for the join table. Code First can configure the many-to-many relationship for you without this code, but if you don't call it, you will get default names such as InstructorInstructorID for the InstructorID column.
          modelBuilder.Entity<Course>()
          .HasMany(c => c.Instructors).WithMany(i => i.Courses)
          .Map(t => t.MapLeftKey("CourseID")
          .MapRightKey("InstructorID")
           .ToTable("CourseInstructor"));
        */

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors).WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID").MapRightKey("InstructorID")
                .ToTable("CourseInstructor"));
            //This code instructs Entity Framework to use stored procedures for insert, update, and delete operations on the Department entity. 
            modelBuilder.Entity<Department>().MapToStoredProcedures();
            // Using the fluent API and IsConcurrencyToken method to specify the tracking property
            modelBuilder.Entity<Department>().Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }
}


