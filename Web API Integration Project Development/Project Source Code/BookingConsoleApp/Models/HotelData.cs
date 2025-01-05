// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the HotelData class, representing hotel information
    public class HotelData
    {
        // Property to store the destination ID
        public string DestId { get; set; }

        // Property to define the type of search being performed
        public string SearchType { get; set; }

        // Property to store the latitude of the hotel location
        public double Latitude { get; set; }

        // Property for the location code (abbreviation)
        public string Lc { get; set; }

        // Property to indicate if the search is roundtrip
        public string Roundtrip { get; set; }

        // Property to store the URL of the hotel image
        public string ImageUrl { get; set; }

        // Property to define the type of the hotel (e.g., luxury, budget)
        public string Type { get; set; }

        // Property to describe the type of destination (e.g., city, beach)
        public string DestType { get; set; }

        // Property to store the number of hotels found in the search
        public int Hotels { get; set; }

        // Property to specify the region where the hotel is located
        public string Region { get; set; }

        // Property for labeling the hotel or destination
        public string Label { get; set; }

        // Property to store the longitude of the hotel location
        public double Longitude { get; set; }

        // Property to store the name of the hotel
        public string Name { get; set; }

        // Property to store the city name where the hotel is located
        public string CityName { get; set; }

        // Property to specify the country where the hotel is located
        public string Country { get; set; }

        // Property to store the number of hotels available in the search result
        public int NrHotels { get; set; }

        // Property for the unique identifier for the city
        public string CityUfi { get; set; }

        // Property to store the country code (ISO format)
        public string Cc1 { get; set; }
    }
}