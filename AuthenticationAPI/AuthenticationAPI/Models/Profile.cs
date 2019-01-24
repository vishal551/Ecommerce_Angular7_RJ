using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationAPI.Models
{
    public class Profile
    {       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ProfilePicture { get; set; }
    }
}