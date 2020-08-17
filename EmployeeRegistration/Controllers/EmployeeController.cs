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
    public class EmployeeController : ApiController
    {
        
      // Get Method to collect all the employee Details.
      // BaseUrl/api/Employee/
        public HttpResponseMessage Get()
        {
            using (RencataEntities db = new RencataEntities()) // DBContext object created inside using statement to destroy once its purpose finihsed.
            {
                var Employees = db.EmployeeInfoes.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, Employees);
            }
        }

        // Get Method to get employee details by Id 
        // IF employee found return with employee details else return not found message along with statuscode.
        public HttpResponseMessage Get(int id)
        {
            using (RencataEntities db = new RencataEntities())
            {
                var entity = db.EmployeeInfoes.FirstOrDefault(e => e.EmpID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with ID " + id.ToString() + " not found");
                }
            }

        }

        // POST Method to create Employee Details.
        // If Employee added successfully return with statuscode created and empid else return bad request message with exception details.
        public HttpResponseMessage Post([FromBody] EmployeeInfo employee)
        {
            try
            {
                using (RencataEntities db = new RencataEntities())
                {
                    var entity = db.EmployeeInfoes.Count(e => e.Firstname == employee.Firstname && e.Lastname == employee.Lastname && e.Contactno == employee.Contactno);
                    if (entity == 0)
                    {
                        if (ModelState.IsValid)
                        {
                            db.EmployeeInfoes.Add(employee);// employee details added to model
                            db.SaveChanges();// model is saved
                            var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                            message.Headers.Location = new Uri(Request.RequestUri +
                                employee.EmpID.ToString());
                            return message;
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Validation Failed");                            
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Found, "Employee Already Exist with same information.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // Put Method to update employee details.
        public HttpResponseMessage Put(int id, [FromBody] EmployeeInfo employee)
        {
            try
            {
                using (RencataEntities db = new RencataEntities())
                {                    
                    if (!EmployeeInfoExists(id))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(employee).State = EntityState.Modified;
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, employee);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NoContent,"Validation Failed");                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        // Delete Method to Delete the Employee Details
        // Delete operation performed in two ways. one is hard delete and another is soft delete.
        // Soft delete is performed by changing status column in db. which is not followed here.
        // Here We follow hard delete -  by deleteing whole record from db.
        
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (RencataEntities db = new RencataEntities())
                {
                    var entity = db.EmployeeInfoes.FirstOrDefault(e => e.EmpID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Employee with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        db.EmployeeInfoes.Remove(entity);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // Function to check Employee is already Exist using ID
        private bool EmployeeInfoExists(int id)
        {
            using (RencataEntities db = new RencataEntities())
            {
                return db.EmployeeInfoes.Count(e => e.EmpID == id) > 0;
            }
        }
    }
}