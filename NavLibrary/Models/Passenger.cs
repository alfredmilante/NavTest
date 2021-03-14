using System;
using System.Collections.Generic;
using System.Text;

namespace NavLibrary.Models
{
    public class Passenger
    {
        public string PNR { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age
        {
            get
            {
                var age = DateTime.Now.Year - BirthDate.Year;

                if (BirthDate.Date > DateTime.Now.AddYears(-age)) age--;

                return age;
            }
        }
    }
}
