using EmployeeRegistrationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using EmployeeRegistration.Models;
using Newtonsoft.Json;
using System.Net;

namespace EmployeeRegistrationApp.Controllers
{
    public class EmployeeController : Controller
    {
        private RencataEntities db = new RencataEntities();
        // GET: Employee
        public ActionResult Index()
        {
            IEnumerable<EmployeeDetails> empList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Employee").Result;
            empList = response.Content.ReadAsAsync<IEnumerable<EmployeeDetails>>().Result;
            return View(empList);
        }

        public ActionResult Create()
        {
            ViewBag.State = new SelectList(db.States, "StateName", "StateName"); 
            ViewBag.Gender = new SelectList(new List<SelectListItem>
                                                    {
                                                        new SelectListItem { Value="Male", Text="Male" },
                                                        new SelectListItem { Value="Female", Text="Female" },
                                                    }, "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeeDetails emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Employee", emp).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Saved Successfully";
                    }
                    else
                    {
                        TempData["Failure"] = response.Content.ReadAsStringAsync().Result;// "Employee Already Exist! or Error message ";
                    }
                }
                else
                {
                    TempData["Failure"] = "Employee Insertion Failed!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Failure"] = "Error :" + ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Employee/" + id.ToString()).Result;

            
            var empList = response.Content.ReadAsStringAsync().Result;
            EmployeeDetails emp = JsonConvert.DeserializeObject<EmployeeDetails>(empList);
            ViewBag.State = new SelectList(db.States, "StateName", "StateName", emp.State);
            ViewBag.Gender = new SelectList(new List<SelectListItem>
                                                    {
                                                        new SelectListItem { Value="Male", Text="Male" },
                                                        new SelectListItem { Value="Female", Text="Female" },
                                                    }, "Value", "Text",emp.Gender);
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeDetails emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Employee/" + emp.EmpID, emp).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Updated Successfully";
                    }
                    else
                    {
                        TempData["Failure"] = response.Content.ReadAsStringAsync().Result;
                    }
                }
                else
                {
                    TempData["Failure"] = "Update Failed!- Validation Failed";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Failure"] = "Error :" + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Employee/" + id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Deleted Successfully";
            }
            else
            {
                TempData["Failure"] = "Delete Record Failed-" + response.Content.ReadAsStringAsync().Result;
            }
            return RedirectToAction("Index");
        }

    }
}