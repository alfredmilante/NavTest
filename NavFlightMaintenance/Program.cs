using NavLibrary;
using NavLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NavFlightMaintenance
{
    class Program
    {
        static CsvFileWriter csvFileWriter;
        static CsvFileReader csvFileReader;
        static void Main(string[] args)
        {

        // Add flight or search flight

        start:
            Console.Clear();
            Console.WriteLine("NAVITAIRE  FLIGHT MAINTENRANCE \n 1 - Add Flight \n 2 - Search Flight \n TYPE NUMBER FOR SELECTION:");

            var isValid = int.TryParse(Console.ReadLine(), out int selection);

            if (isValid && selection == 1)
            {
            addFlight:
                Console.WriteLine("ADD FLIGHT \n Please provide information");
                Console.WriteLine("Airline Code: ");
                var airlineCode = Console.ReadLine();

                if (string.IsNullOrEmpty(airlineCode) ||
                    airlineCode.Length != 2 ||
                    int.TryParse(airlineCode, out int code))
                {
                    goto addFlight;
                }

            setFlightNumber:

                Console.WriteLine("Flight Number: ");
                var flightNumber = Console.ReadLine();

                if (!int.TryParse(flightNumber, out int number) ||
                    number < 1 ||
                    number > 9999)
                {
                    goto setFlightNumber;
                }

            setArrivalStation:

                Console.WriteLine("Arrival Station: ");
                var arrivalStation = Console.ReadLine();

                if (int.TryParse(arrivalStation, out int aStation) ||
                    arrivalStation.Length != 3 ||
                    int.TryParse(arrivalStation[0].ToString(), out int aStationFirstChar))
                {
                    goto setArrivalStation;
                }

            setDepartureStation:

                Console.WriteLine("Departure Station: ");
                var departureStation = Console.ReadLine();

                if (int.TryParse(departureStation, out int dStation) ||
                    departureStation.Length != 3 ||
                    int.TryParse(departureStation[0].ToString(), out int dStationFirstChar))
                {
                    goto setDepartureStation;
                }

            setSTA:
                Console.WriteLine("STA: ");
                var sta = Console.ReadLine();

                if (!Regex.IsMatch(sta, "^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$"))
                {
                    goto setSTA;
                }

            setSTD:
                Console.WriteLine("STD: ");
                var std = Console.ReadLine();

                if (!Regex.IsMatch(std, "^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$"))
                {
                    goto setSTD;
                }

                Console.WriteLine(string.Format("Details: \n" +
                    "Airline Code: {0} \n" +
                    "Flight Number: {1} \n" +
                    "Arrival Station: {2} \n" +
                    "Departure Station: {3} \n" +
                    "STA: {4} \n" +
                    "STD: {5} \n", airlineCode, flightNumber, arrivalStation, departureStation, sta, std));

                // CSV Add
                if(!AddFlight(new Flight()
                {
                    AirlineCode = airlineCode,
                    FlightNumber = int.Parse(flightNumber),
                    ArrivalStation = arrivalStation,
                    DepartureStation = departureStation,
                    STA = TimeSpan.Parse(sta),
                    STD = TimeSpan.Parse(std)
                }))
                {
                    Console.WriteLine("Flight not added. Either duplicate flight or unknown error.");
                    goto addFlight;
                }    

            }
            else if (isValid && selection == 2)
            {
            search:
                Console.Clear();
                Console.WriteLine("SEARCH BY: \n 1 - Flight Number \n 2 - Airline Code \n 3 - Origin/Destination");

                isValid = int.TryParse(Console.ReadLine(), out selection);

                List<Flight> searchedFlights;

                if (isValid && selection == 1)
                {
                searchByFlightNumber:
                    Console.WriteLine("Search for Flight Number: ");
                    if (int.TryParse(Console.ReadLine(), out int searchedFlightNumber))
                    {
                        searchedFlights = SearchByFlightNumber(searchedFlightNumber);
                    }
                    else
                    {
                        goto searchByFlightNumber;
                    }
                }
                else if (isValid && selection == 2)
                {
                    Console.WriteLine("Search for Airline Code: ");

                    searchedFlights = SearchByAirlineCode(Console.ReadLine());
                }
                else if (isValid && selection == 3)
                {
                    Console.WriteLine("Search for Origin/Destination: ");
                    searchedFlights = SearchByDestinationOrArrival(Console.ReadLine());
                }
                else
                {
                    Console.WriteLine("Incorrect selection.  Please try again.");
                    goto search;
                }

                Console.WriteLine("Search Result: ");
                foreach (var flight in searchedFlights)
                {
                    Console.WriteLine(string.Format(
                 "Airline Code: {0}, " +
                 "Flight Number: {1}, " +
                 "Arrival Station: {2}, " +
                 "Departure Station: {3}, " +
                 "STA: {4}, " +
                 "STD: {5} \n", flight.AirlineCode, flight.FlightNumber, flight.ArrivalStation, flight.DepartureStation, flight.STA, flight.STD));
                }
            }
            else
            {
                Console.WriteLine("Incorrect selection.  Please try again.");
                goto start;
            }
        }

        private static bool AddFlight(Flight flight)
        {
            csvFileWriter = new CsvFileWriter(@"C:\Users\jmilante\Desktop\flights.txt");
            var existingFlights = GetFlights();

            if (existingFlights.FirstOrDefault(f =>
             f.AirlineCode == flight.AirlineCode &&
             f.FlightNumber == flight.FlightNumber &&
             f.DepartureStation == flight.DepartureStation &&
             f.ArrivalStation == flight.ArrivalStation) == null)
            {
                try
                {
                    var row = new List<string>()
                {
                  flight.AirlineCode,
                  flight.FlightNumber.ToString(),
                    flight.ArrivalStation,
                    flight.DepartureStation,
                    flight.STA.ToString(),
                    flight.STD.ToString()
                };

                    csvFileWriter.WriteRow(row);

                    return true;
                }

                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        private static List<Flight> GetFlights()
        {
            csvFileReader = new CsvFileReader(@"C:\Users\jmilante\Desktop\flights.txt");

            var flightCsv = csvFileReader.ReadCsv();

            List<Flight> flights = new List<Flight>();

            foreach (var flight in flightCsv)
            {
                try { 
                var columns = flight.Split(',');

                flights.Add(new Flight()
                {
                    AirlineCode = columns[0],
                    FlightNumber = int.Parse(columns[1]),
                    ArrivalStation = columns[2],
                    DepartureStation = columns[3],
                    STA = TimeSpan.Parse(columns[4]),
                    STD = TimeSpan.Parse(columns[5])
                });
                }
                catch { 
                }
            }

            return flights;
        }

        private static List<Flight> SearchByFlightNumber(int flightNumber)
        {
            var flights = GetFlights();
            return flights.Where(f => f.FlightNumber == flightNumber).ToList();
        }
        private static List<Flight> SearchByAirlineCode(string airlineCode)
        {
            var flights = GetFlights();
            return flights.Where(f => f.AirlineCode == airlineCode).ToList();
        }
        private static List<Flight> SearchByDestinationOrArrival(string arrivalOrDestination)
        {
            var flights = GetFlights();
            return flights.Where(f => f.ArrivalStation == arrivalOrDestination || f.DepartureStation == arrivalOrDestination).ToList();
        }
    }
}


