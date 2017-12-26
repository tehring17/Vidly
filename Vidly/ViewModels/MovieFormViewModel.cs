using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }  //Using IEnumerable, because we just need to iterate through the Genres.  If we needed to utilize the functionality associated with a List<>, then we would have defined like that.

        //public Movie Movie { get; set; }  --You can either create a property based on the entire class and pass the model to it, or use a "Pure View Model"... you can replace with individual properties:
        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public byte? GenreId { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Number In Stock")]
        [Range(0, 20, ErrorMessage = "Enter number between 0-20")]
        [Required]
        public byte? NumberInStock { get; set; }

        public string Title
        {
            get
            {
                //(This is what we used when we were passing the Movie Property from the controller)  if (Movie != null && Movie.Id != 0)

                //if (Id != 0)
                //    return "Edit Movie";

                //return "New Movie";
                // OR, one line of code like this (which is a Turnary operator):
                return Id != 0 ? "Edit Movie" : "New Movie";
            }
        }

        public MovieFormViewModel()
        {
            Id = 0;  //When making a call for a new movie, we want to make sure the hidden field is populated with the Id of 0 (instead of nothing)
        }

        public MovieFormViewModel(Movie movie)
        {
            Id = movie.Id;
            Name = movie.Name;
            ReleaseDate = movie.ReleaseDate;
            NumberInStock = movie.NumberInStock;
            GenreId = movie.GenreId;
        }
    }
}