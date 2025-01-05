// Importing the System namespace for general functionalities
using System;

// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the HotelReview class, representing a review for a hotel
    public class HotelReview
    {
        // Property to store the country code of the reviewer
        public string CountryCode { get; set; }

        // Property to store the language code of the review
        public string LanguageCode { get; set; }

        // Property to store positive aspects of the hotel as mentioned in the review
        public string Pros { get; set; }

        // Property to store negative aspects of the hotel as mentioned in the review
        public string Cons { get; set; }

        // Property to store the average score given by the reviewer
        public int AverageScore { get; set; }

        // Property to store the title of the review
        public string Title { get; set; }

        // Property to indicate the purpose of travel as stated by the reviewer
        public string TravelPurpose { get; set; }

        // Property to store the date when the review was written
        public string Date { get; set; }
    }
}