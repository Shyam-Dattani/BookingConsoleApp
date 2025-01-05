// Importing the System.Collections.Generic namespace for using generic collections
using System.Collections.Generic;

// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the HotelPhoto class, representing a photo of a hotel
    public class HotelPhoto
    {
        // Property to store the unique identifier for the hotel photo
        public int Id { get; set; }

        // Property to store the URL of the hotel photo
        public string Url { get; set; }
    }

    // Definition of the HotelPhotosResponse class, representing the response structure for hotel photos
    public class HotelPhotosResponse
    {
        // Property to store a list of hotel photos
        public List<HotelPhoto> Data { get; set; }
    }
}