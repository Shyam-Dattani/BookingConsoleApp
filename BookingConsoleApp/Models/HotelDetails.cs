// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the HotelDetails class, representing detailed information about a hotel
    public class HotelDetails
    {
        // Property to store the unique identifier for the hotel
        public int Ufi { get; set; }

        // Property to store the hotel ID
        public int HotelId { get; set; }

        // Property to store the name of the hotel
        public string HotelName { get; set; }

        // Property to store the URL for more details about the hotel
        public string Url { get; set; }

        // Property to store the address of the hotel
        public string Address { get; set; }

        // Property to store the city where the hotel is located
        public string City { get; set; }

        // Property to store the country code where the hotel is located
        public string CountryCode { get; set; }

        // Property to store the number of available rooms in the hotel
        public int AvailableRooms { get; set; }

        // Property to store the average room size of the hotel
        public string AverageRoomSize { get; set; }
    }

    // Definition of the HotelDetailsResponse class, representing the response structure for hotel details
    public class HotelDetailsResponse
    {
        // Property to indicate the status of the API response
        public bool Status { get; set; }

        // Property to store any message associated with the response
        public string Message { get; set; }

        // Property to store the timestamp of the API response
        public long Timestamp { get; set; }

        // Property to store the hotel details data
        public HotelDetails Data { get; set; }
    }
}