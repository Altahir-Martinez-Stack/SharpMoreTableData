using MyBusiness.Areas.Users.ViewModels;
using MyBusiness.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Linq.Dynamic;
using System.Web.Services.Description;

namespace MyBusiness.Areas.Users.Controllers
{
    public class EmployeeController : Controller
    {
        private MyBusinessDataEntities db = new MyBusinessDataEntities();

        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        // GET: Users/Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Json()
        {
            List<EmployeeViewModel> lst = new List<EmployeeViewModel>();

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;


            using (db)
            {
                IQueryable<EmployeeViewModel> query = (from d in db.Employee
                                                       select new EmployeeViewModel
                                                       {
                                                           id = d.id,
                                                           name = d.name,
                                                           lastName = d.lastName,
                                                           surName = d.surName,
                                                           gender = d.gender,
                                                           email = d.email,
                                                           createDate = d.createDate,
                                                       });

                if (!String.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(t => t.name.Contains(searchValue));
                }

                if (!(String.IsNullOrEmpty(sortColumn) && String.IsNullOrEmpty(sortColumnDir)))
                {
                    query = query.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = query.Count();
                lst = query.Skip(skip).Take(pageSize).ToList();
            }

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = lst });
        }
        [HttpGet]
        public JsonResult GetEmployeeById(int id)
        {
            try
            {
                var employee = db.Employee.Where(t => t.id == id).FirstOrDefault();
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public JsonResult Create(EmployeeViewModel employee)
        {
            try
            {
                var result = false;

                if (employee.name != null)
                {
                    Employee employeeCreate = new Employee();

                    employeeCreate.name = employee.name;
                    employeeCreate.lastName = employee.lastName;
                    employeeCreate.surName = employee.surName;
                    employeeCreate.gender = employee.gender;
                    employeeCreate.email = employee.email;
                    employeeCreate.createDate = DateTime.Now;

                    db.Employee.Add(employeeCreate);
                    db.SaveChanges();


                    result = true;

                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }
            
            }
            catch (Exception e)
            {

                var messageError = e.Message;
                return Json(messageError, JsonRequestBehavior.AllowGet);
            }

           

        }
        [HttpPost]
        public JsonResult Update(EmployeeViewModel employee)
        {
            try
            {
                bool result = false;
                var employeeUpdate = db.Employee.Where(t => t.id == employee.id).FirstOrDefault();

                if (employeeUpdate != null)
                {
                    if (!String.IsNullOrEmpty(employee.name))
                    {
                        employeeUpdate.name = employee.name;
                    }

                    if (!String.IsNullOrEmpty(employee.lastName))
                    {
                        employeeUpdate.lastName = employee.lastName;
                    }

                    if (!String.IsNullOrEmpty(employee.surName))
                    {
                        employeeUpdate.surName = employee.surName;
                    }

                    if (!String.IsNullOrEmpty(employee.gender))
                    {
                        employeeUpdate.gender = employee.gender;
                    }

                    if (!String.IsNullOrEmpty(employee.email))
                    {
                        employeeUpdate.email = employee.email;
                    }

                    db.SaveChanges();

                    result = true;
                }

                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var messageError = e.Message;
                return Json(messageError, JsonRequestBehavior.AllowGet);
            }
           
        }
        public JsonResult Delete(int? id)
        {
            var result = false;

            if (id != null)
            {
                var employeeDelete = db.Employee.Where(t => t.id == id).FirstOrDefault();

                if (employeeDelete != null)
                {
                    db.Employee.Remove(employeeDelete);
                    db.SaveChanges();

                    result = true;
                }

                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
    }
}

 