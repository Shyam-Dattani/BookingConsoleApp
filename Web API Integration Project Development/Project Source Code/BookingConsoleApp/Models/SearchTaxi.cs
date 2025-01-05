// Namespace declaration for the BookingConsoleApp.Models
namespace BookingConsoleApp.Models
{
    // Definition of the TaxiOffer class, representing details of a taxi offer
    public class TaxiOffer
    {
        // Property to store the unique identifier for the taxi offer
        public string ResultId { get; set; }

        // Property to store the type of vehicle offered for the taxi service
        public string VehicleType { get; set; }

        // Property to store a description of the taxi offer
        public string Description { get; set; }

        // Property to store the name of the supplier providing the taxi service
        public string SupplierName { get; set; }

        // Property to store the maximum passenger capacity of the taxi
        public int PassengerCapacity { get; set; }

        // Property to store the estimated duration of the taxi ride in minutes
        public int Duration { get; set; }

        // Property to store the URL of the taxi image
        public string ImageUrl { get; set; }

        // Property to indicate if a meet and greet service is provided
        public bool MeetGreet { get; set; }

        // Property to store the price amount for the taxi service
        public decimal PriceAmount { get; set; }

        // Property to store the currency in which the price is quoted
        public string Currency { get; set; }

        // Property to indicate if the taxi offer is non-refundable
        public bool NonRefundable { get; set; }

        // Property to store the driving distance covered by the taxi service in kilometers
        public decimal DrivingDistance { get; set; }
    }
}