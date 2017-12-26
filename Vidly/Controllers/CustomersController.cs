using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        //public ViewResult Index()
        //{
        //    var customers = GetCustomers();
        //    return View(customers);
        //}

        //public ActionResult Details(int id)
        //{
        //    var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

        //    if (customer == null)
        //        return HttpNotFound();

        //    return View(customer);
        //}

        //private IEnumerable<Customer> GetCustomers()
        //{
        //    return new List<Customer>
        //    {
        //        new Models.Customer { Id = 1, Name = "John Smith" },
        //        new Models.Customer { Id = 2, Name = "Mary Williams" }
        //    };
        //}

        private ApplicationDbContext _context;

        // Type 'ctor' {tab}{tab}
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // DbContext is a disposible object, so we have to dispose of it properly...
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ViewResult Index()
        {
            //var customers = _context.Customers.ToList();     --This was commented out so we can see an example of "Eager Loading" with the "Include" below...
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();    //In order to add the Include, your must add import to:  using System.Data.Entity;  
            return View(customers);
        }

        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();  // Remember that MembershipTypes needed to be added to the ApplicationDbContext in IdentityModels.cs
            var viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),     //We are creating here so that the properties are initialzed to their default values - Ex:  The Id which is stored in hidden field will be initialized to 0 and forms validation error will not fire due to it being NULL.
                MembershipTypes = membershipTypes
            };
            return View("CustomerForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }

        /**********************
        Remember the snippet to create a new action is, "mvcaction4"
        ***********************/
        public ActionResult Details(int id)
        {
            //(Before eager loading added for MembershipType)  var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        
        [HttpPost]   //The HttpPost attribute is specified for this action, because it should only apply when Posting and not during HttpGet!  As a best practice, if actions modify data, they should only be accessible by a POST and never a GET.
        [ValidateAntiForgeryToken]     //This will validate the token defined on the respective Form.  To protect against "Cross-site Request Forgery"
        public ActionResult Save(Customer customer)
        {
            //Use ModelState property to get access to validation data (based on annotations found in the respective class object)
            if (!ModelState.IsValid)     //If ModelState is NOT valid, then return the same view!
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("CustomerForm", viewModel);
            }

            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                //  TryUpdateModel(customerInDb);  //This is the official way to update the database based on the properties of this customer object will be updated based on the key/value pairs in request data.  This opens up security holes because all fields are updated (if the user is authorized or not).
                //  TryUpdateModel(customerInDb, "", new string[] { "Name", "Email" });   // Microsoft gives us an option to white list fields based on a list in the third parm, but this is not good in case something gets renamed later.
                // --OR, specifiy the respective fields to update...--
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}