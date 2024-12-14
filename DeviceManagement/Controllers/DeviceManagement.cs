using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeviceManagement.Controllers
{
    public class DeviceManagementController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public ActionResult Create(T_Customers model)
        {
            using (var context = new DeviceManagementEntities2()) //To open a connection to the database
            {
                var data = context.T_Customers.FirstOrDefault(x => x.CustomerID == model.CustomerID);

                // Checking user is duplicate or not
                if (data != null)
                {
                    string message1 = "Customer ID is duplicate";
                    ViewBag.Message = message1;
                    return View();
                }

                model.RegistDateTime = DateTime.Now;
                context.T_Customers.Add(model); // Add data to the particular table
                context.SaveChanges(); // save the changes to the that are made
            }

            string message = "Created the record successfully";
            ViewBag.Message = message; // To display the message on the screen after the record is created successfully
            return View(); 
        }

        [HttpGet] // Set the attribute to Read
        public ActionResult Read()
        { 
            using (var context = new DeviceManagementEntities2())
            {
                var data = context.T_Customers.ToList();
                return View(data);
            }
        }

        public ActionResult Update(int id) // To fill data in the form to enable easy editing
        {
            using (var context = new DeviceManagementEntities2())
            {
                var data = context.T_Customers.Where(x => x.ID == id).SingleOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // To specify that this will be invoked when post method is called
        public ActionResult Update(int id, T_Customers model)
        {
            using (var context = new DeviceManagementEntities2())
            {
                var data = context.T_Customers.FirstOrDefault(x => x.ID == id);
                
                if (data != null) // Checking if any such record exist 
                {
                    // data.CustomerID = model.CustomerID;
                    data.Name = model.Name;
                    data.RegistDateTime = DateTime.Now;
                    context.SaveChanges();
                    return RedirectToAction("Read"); // It will redirect to the Read method
                }
                else
                    return View();
            }
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string customerId)
        {
            using (var context = new DeviceManagementEntities2())
            {
                var data1 = context.T_Customers.FirstOrDefault(x => x.ID == id);
                var data2 = context.T_Customers.FirstOrDefault(x => x.CustomerID == customerId);

                if (data1 != null) // Checking if any such record exist 
                {
                    context.T_Customers.Remove(data1);
                    if (data2 != null)
                    {
                        context.T_Customers.Remove(data2);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Read");
                }
                else
                    return View();
            }
        }
    }
}