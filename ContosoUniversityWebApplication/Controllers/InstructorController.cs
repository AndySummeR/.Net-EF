﻿using System.Linq;
using System.Web.Mvc;
using ContosoUniversityWebApplication.DAL;
using ContosoUniversityWebApplication.ViewModels;
using System.Data.Entity;
using ContosoUniversityWebApplication.Models;
using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Collections.Generic;

namespace ContosoUniversityWebApplication.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();
        // GET: Instructor

        /* The method accepts optional route data (id)  and a query string parameter (courseID) that provide the ID values of the selected instructor and selected course, and passes all of the required data to the view. 
         * The parameters are provided by the Select hyperlinks on the page.
         */
        public ActionResult Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors.Include(i => i.OfficeAssignment).Include(i => i.Courses.Select(c => c.Department)).OrderBy(i => i.LastName);
            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                //The Single method converts the collection into a single Instructor entity, which gives you access to that entity's Courses property. 
                //You use the Single method on a collection when you know the collection will have only one item. 
                viewModel.Courses = viewModel.Instructors.Where(i => i.ID == id.Value).Single().Courses;
                //viewModel.Courses = viewModel.Instructors.Single(i => i.ID == id.Value).Courses; 
            }
            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                //Lazy loading
                viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading     
                //Notice that you use the Collection method to load a collection property, but for a property that holds just one entity, you use the Reference method.
                //var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                //db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                //foreach (Enrollment enrollment in selectedCourse.Enrollments)
                //{
                //    db.Entry(enrollment).Reference(x => x.Student).Load();
                //}
                //viewModel.Enrollments = selectedCourse.Enrollments;
            }
            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.ID == id).Single();
            PopulateAssignedCourseData(instructor);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }


        /*
         *  For each course, the code checks whether the course exists in the instructor's Courses navigation property. 
         *  To create efficient lookup when checking whether a course is assigned to the instructor,
         *  the courses assigned to the instructor are put into a HashSet collection. 
         */
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID) //Contains() --bool type. The view will use this property to determine which check boxes must be displayed as selected. 
                });
            }
            ViewBag.Courses = viewModel;
        }



        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var instructorToUpdate = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .Where(i => i.ID == id)
                .Single();

            if (TryUpdateModel(instructorToUpdate, "", new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }

                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                    db.Entry(instructorToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {          //Log the error (uncomment dex variable name and add a line here to write a log.          
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedCourseData(instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.CourseID));
            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Remove(course);
                    }
                }
            }
        }


        // GET: Instructor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}