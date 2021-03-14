using System;
using System.Collections.Generic;
using System.Text;

namespace NavLibrary.Models
{
    public class Reservation
    {
        public string AirlineCode { get; set; }

        public int FlightNumber { get; set; }

        public DateTime FlightDate { get; set; }

        public int NoOfPassengers { get; set; }

        public string PNR { get; set; }
    }
}
