// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the SearchHotels class, representing a hotel in search results
    public class SearchHotels
    {
        // Property to store the unique identifier for the hotel
        public int HotelId { get; set; }

        // Property to store the name of the hotel
        public string Name { get; set; }

        // Property to store accessibility information for the hotel
        public string AccessibilityLabel { get; set; }

        // Property to store the price of the hotel
        public double Price { get; set; }

        // Property to store the currency in which the price is stated
        public string Currency { get; set; }

        // Property to store the rating of the hotel, usually on a scale (e.g., 1-10)
        public double Rating { get; set; }

        // Property to store a description of the room offered at the hotel
        public string RoomDescription { get; set; }

        // Property to store the URL for the hotel's image
        public string ImageUrl { get; set; }

        // Property to store the current status of the hotel (e.g., available, booked)
        public string Status { get; set; }
    }
}