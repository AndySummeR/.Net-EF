using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using ContosoUniversityWebApplication.DAL;
using ContosoUniversityWebApplication.Models;
using System.Data.Entity.Infrastructure;
using System;
using System.Linq;
using System.Data;
using log4net;

namespace ContosoUniversityWebApplication.Controllers
{
    public class DepartmentController : Controller
    {
        private SchoolContext db = new SchoolContext();
        

        private static ILog Log { get; set; }
        ILog log = LogManager.GetLogger(typeof(DepartmentController));

        // GET: Department
        /*
         *  Optimistic concurrency means allowing concurrency conflicts to happen, and then reacting appropriately if they do.
         * 
         *  With synchronous code, many threads may be tied up while they aren’t actually doing any work because they’re waiting for I/O to complete. 
         *  With asynchronous code, when a process is waiting for I/O to complete, its thread is freed up for the server to use for processing other requests.
         * 
         * If you want to take advantage of the performance benefits of async code, make sure that any library packages that you're using (such as for paging), 
         * also use async if they call any Entity Framework methods that cause queries to be sent to the database. 
         */
        public async Task<ActionResult> Index()
        {
            log.Debug("Debug message");
            log.Info("Info message");
            log.Warn("Warn message");
            log.Error("Error message");
            log.Fatal("Fatal message");

            var departments = db.Departments.Include(d => d.Administrator);
            return View(await departments.ToListAsync());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(department).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
        //    return View(department);
        //}
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentID, Name, Budget, StartDate, RowVersion, InstructorID")] Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ValidateOneAdministratorAssignmentPerInstructor(department);
                }

                if (ModelState.IsValid)
                {
                    db.Entry(department).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            /*
             If no rows are affected by the UPDATE command (no rows have the original RowVersion value),  
             the Entity Framework throws a DbUpdateConcurrencyException exception, 
             and the code in the catch block gets the affected Department entity from the exception object.
             */
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Department)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if (databaseEntry == null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Department)databaseEntry.ToObject();

                    if (databaseValues.Name != clientValues.Name)
                        ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);
                    if (databaseValues.Budget != clientValues.Budget)
                        ModelState.AddModelError("Budget", "Current value: " + String.Format("{0:c}", databaseValues.Budget));
                    if (databaseValues.StartDate != clientValues.StartDate)
                        ModelState.AddModelError("StartDate", "Current value: " + String.Format("{0:d}", databaseValues.StartDate));
                    if (databaseValues.InstructorID != clientValues.InstructorID)
                        ModelState.AddModelError("InstructorID", "Current value: " + db.Instructors.Find(databaseValues.InstructorID).FullName);

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                        + "was modified by another user after you got the original value. The "
                        + "edit operation was canceled and the current values in the database "
                        + "have been displayed. If you still want to edit this record, click "
                        + "the Save button again. Otherwise click the Back to List hyperlink.");
                    department.RowVersion = databaseValues.RowVersion;
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.         
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            ViewBag.InstructorID = new SelectList(db.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Department/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Department department = await db.Departments.FindAsync(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(department);
        //}

        public ActionResult Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (department == null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                        + "was deleted by another user after you got the original values. "
                        + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                        + "was modified by another user after you got the original values. "
                        + "The delete operation was canceled and the current values in the "
                        + "database have been displayed. If you still want to delete this "
                        + "record, click the Delete button again. Otherwise "
                        + "click the Back to List hyperlink.";
                }
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Department department = await db.Departments.FindAsync(id);
        //    db.Departments.Remove(department);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(Department department)
        {
            try
            {
                db.Entry(department).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true });
            }
            catch (DataException /* dex */)
            {         //Log the error (uncomment dex variable name after DataException and add a line here to write a log.         
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(department);
            }
        }

        private void ValidateOneAdministratorAssignmentPerInstructor(Department department)
        {
            if (department.InstructorID != null)
            {
                Department duplicateDepartment = db.Departments.Include("Administrator")
                    .Where(d => d.InstructorID == department.InstructorID)
                    .AsNoTracking()
                    .FirstOrDefault();
                if (duplicateDepartment != null && duplicateDepartment.DepartmentID != department.DepartmentID)
                {
                    string errorMessage = String.Format("Instructor {0} {1} is already administrator of the {2} department.", 
                        duplicateDepartment.Administrator.FirstMidName, 
                        duplicateDepartment.Administrator.LastName, 
                        duplicateDepartment.Name);
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
