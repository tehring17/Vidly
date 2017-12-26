using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; }   //Using IEnumerable, because we just need to iterate through the membership types.  If we needed to utilize the functionality associated with a List<>, then we would have defined like that.
        public Customer Customer { get; set; }
    }
}