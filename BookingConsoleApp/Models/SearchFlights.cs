// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the FlightOffer class, representing details of a flight offer
    public class FlightOffer
    {
        // Property to store a unique key identifying the flight offer
        public string Key { get; set; }

        // Property to store a token for the flight offer
        public string OfferToken { get; set; }

        // Property to store the price of the flight offer, nullable to handle cases where the price may not be set
        public int? Price { get; set; }

        // Property to store the currency in which the price is quoted
        public string Currency { get; set; }

        // Property to store the minimum duration of the flight in minutes, nullable for optional value
        public int? DurationMin { get; set; }

        // Property to store the maximum duration of the flight in minutes, nullable for optional value
        public int? DurationMax { get; set; }
    }
}