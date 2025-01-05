// Importing necessary namespace for using generic collections
using System.Collections.Generic;

// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the RoomAvailability class, representing room availability details
    public class RoomAvailability
    {
        // Property to store the currency used for pricing
        public string Currency { get; set; }

        // Property to store the length of stay in days
        public int LengthsOfStay { get; set; }

        // Dictionary to store available dates and their corresponding prices
        // Key: Date as a string, Value: Price as a decimal
        public Dictionary<string, decimal> AvDates { get; set; } = new Dictionary<string, decimal>();
    }
}