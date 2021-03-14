using System;
using System.Collections.Generic;
using System.Text;

namespace NavLibrary.Models
{
    public class Flight
    {
        public string AirlineCode { get; set; }

        public int FlightNumber { get; set; }

        public string ArrivalStation { get; set; }

        public string DepartureStation { get; set; }

        public TimeSpan STA { get; set; }
        public TimeSpan STD { get; set; }

    }
}
