using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        //public ViewResult Index()
        //{
        //    var movies = GetMovies();
        //    return View(movies);
        //}

        //private IEnumerable<Movie> GetMovies()
        //{
        //    return new List<Movie>
        //    {
        //        new Movie { Id = 1, Name = "Shrek" },
        //        new Movie { Id = 2, Name = "Wall-e" }
        //    };
        //}

        //// GET: Movies/Random
        //public ActionResult Random()
        //{

        //    var movie = new Movie() { Name = "Shrek!" };
        //    var customers = new List<Customer>
        //    {
        //        new Models.Customer { Name = "Customer 1" },
        //        new Models.Customer { Name = "Customer 2" }
        //    };

        //    var viewModel = new RandomMovieViewModel
        //    {
        //        Movie = movie,
        //        Customers = customers
        //    };

        //    //var ViewResult = new ViewResult();
        //    //ViewResult.ViewData.Model

        //    return View(viewModel);

        //    //return Content("Hello Tom!");
        //    //return HttpNotFound();
        //    //return new EmptyResult();
        //    //return RedirectToAction("Index", "Home", new { page=1, sortBy="name", animal="dog", name="Buster" });
        //}

        //[Route("movies/released/{year}/{month:regex(\\d{2}):range(1, 12)}")]      //This custom route has an example of "ASP.NET MVC Attribute Route Constraints
        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(year + "/" + month);
        //}

        //public ActionResult Edit(int id)
        //{
        //    return Content("id=" + id);
        //}

        // movies
        //public ActionResult index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue)
        //        pageIndex = 1;

        //    if (string.IsNullOrWhiteSpace(sortBy))
        //        sortBy = "Name";

        //    return Content(string.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        //}


        private ApplicationDbContext _context;

        // Type 'ctor' {tab}{tab}
        public MoviesController()
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
            var movies = _context.Movies.Include(m => m.Genre).ToList();    //In order to add the Include, your must add import to:  using System.Data.Entity;  
            return View(movies);
        }
        
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();  // Remember that Genres needed to be added to the ApplicationDbContext in IdentityModels.cs
            var viewModel = new MovieFormViewModel
            {
                //Movie = new Movie(),     //We are creating here so that the properties are initialzed to their default values - Ex:  The Id which is stored in hidden field will be initialized to 0 and forms validation error will not fire due to it being NULL.
                Genres = genres
            };
            return View("MovieForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            var viewModel = new MovieFormViewModel(movie)
            {
                //Movie = movie,
                Genres = _context.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        
        [HttpPost]   //The HttpPost attribute is specified for this action, because it should only apply when Posting and not during HttpGet!  As a best practice, if actions modify data, they should only be accessible by a POST and never a GET.
        [ValidateAntiForgeryToken]     //This will validate the token defined on the respective Form.  To protect against "Cross-site Request Forgery"
        public ActionResult Save(Movie movie)
        {
            //Use ModelState property to get access to validation data (based on annotations found in the respective class object)
            if (!ModelState.IsValid)     //If ModelState is NOT valid, then return the same view!
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    //Movie = movie,
                    Genres = _context.Genres.ToList()
                };
                return View("MovieForm", viewModel);
            }

            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);

                //  TryUpdateModel(moveInDb);  //This is the official way to update the database based on the properties of this movie object will be updated based on the key/value pairs in request data.  This opens up security holes because all fields are updated (if the user is authorized or not).
                //  TryUpdateModel(moveInDb, "", new string[] { "Name", "ReleaseDate" });   // Microsoft gives us an option to white list fields based on a list in the third parm, but this is not good in case something gets renamed later.
                // --OR, specifiy the respective fields to update...--
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }
    }
}
