using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EmployeeRegistration.Models;

namespace EmployeeRegistration.Controllers
{
    public class EmpController : ApiController
    {
        private RencataEntities db = new RencataEntities();

        // GET: api/Emp
        public IQueryable<EmployeeInfo> GetEmployeeInfoes()
        {
            return db.EmployeeInfoes;
        }

        // GET: api/Emp/5
        [ResponseType(typeof(EmployeeInfo))]
        public IHttpActionResult GetEmployeeInfo(int id)
        {
            EmployeeInfo employeeInfo = db.EmployeeInfoes.Find(id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            return Ok(employeeInfo);
        }

        // PUT: api/Emp/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployeeInfo(int id, EmployeeInfo employeeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeInfo.EmpID)
            {
                return BadRequest();
            }

            db.Entry(employeeInfo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Emp
        [ResponseType(typeof(EmployeeInfo))]
        public IHttpActionResult PostEmployeeInfo(EmployeeInfo employeeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeeInfoes.Add(employeeInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employeeInfo.EmpID }, employeeInfo);
        }

        // DELETE: api/Emp/5
        [ResponseType(typeof(EmployeeInfo))]
        public IHttpActionResult DeleteEmployeeInfo(int id)
        {
            EmployeeInfo employeeInfo = db.EmployeeInfoes.Find(id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            db.EmployeeInfoes.Remove(employeeInfo);
            db.SaveChanges();

            return Ok(employeeInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeInfoExists(int id)
        {
            return db.EmployeeInfoes.Count(e => e.EmpID == id) > 0;
        }
    }
}