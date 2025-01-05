using BookingConsoleApp.Models; // Importing models from the BookingConsoleApp.Models namespace
using System.Collections.Generic; // For using List<T>
using System.Net.Http; // For making HTTP requests
using System.Net.Http.Json; // For JSON serialisation and deserialisation
using System.Threading.Tasks; // For asynchronous programming
using System.Text.Json; // For working with JSON data
using Newtonsoft.Json; // For additional JSON functionality, if needed
using System; // For general functionalities

namespace BookingConsoleApp.Services
{
    // BookingService class to handle hotel booking operations
    public class BookingService
    {
        private readonly HttpClient _httpClient; // HttpClient instance for making API calls

        // Constructor to initialise HttpClient with API key
        public BookingService(string apiKey)
        {
            _httpClient = new HttpClient(); // Create a new HttpClient instance
            // Add required headers for authentication
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "booking-com15.p.rapidapi.com");
        }

        // Asynchronous method to search for hotels based on destination
        public async Task<List<HotelData>> SearchHotelsAsync(string destination)
        {
            // Making a GET request to the Booking.com API with the specified destination
            var response = await _httpClient.GetAsync($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={destination}");
            response.EnsureSuccessStatusCode(); // Ensure the request was successful

            // Read the JSON response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Parse the JSON response
            var jsonDocument = JsonDocument.Parse(jsonResponse);
            var hotelList = new List<HotelData>(); // List to store hotel data

            // Adjust the parsing according to the JSON structure
            // Iterate over each hotel in the JSON data array
            foreach (var hotelJson in jsonDocument.RootElement.GetProperty("data").EnumerateArray())
            {
                // Create a new HotelData object and populate its properties from the JSON
                var hotelData = new HotelData
                {
                    DestId = hotelJson.GetProperty("dest_id").GetString(), // Destination ID
                    Name = hotelJson.GetProperty("name").GetString(), // Hotel name
                    Region = hotelJson.GetProperty("region").GetString(), // Region
                    Country = hotelJson.GetProperty("country").GetString(), // Country
                    Latitude = hotelJson.GetProperty("latitude").GetDouble(), // Latitude
                    Longitude = hotelJson.GetProperty("longitude").GetDouble(), // Longitude
                    ImageUrl = hotelJson.GetProperty("image_url").GetString(), // Image URL
                };
                hotelList.Add(hotelData); // Add the hotel data to the list
            }

            return hotelList; // Return the list of hotels
        }

        // Asynchronous method to retrieve hotel details based on hotel ID and stay dates
        public async Task<HotelDetails> GetHotelDetailsAsync(int hotelId, DateTime arrivalDate, DateTime departureDate)
        {
            // Format the arrival and departure dates to the required format (e.g., yyyy-MM-dd)
            string arrival = arrivalDate.ToString("yyyy-MM-dd"); // Format arrival date
            string departure = departureDate.ToString("yyyy-MM-dd"); // Format departure date

            // Construct the API URL with the required query parameters
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?hotel_id={hotelId}&arrival_date={arrival}&departure_date={departure}&adults=1&children_age=1,17&room_qty=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=EUR";

            // Make an asynchronous GET request to the API
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Ensure the request was successful

            // Read the JSON response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Parse the JSON response
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Access the 'data' object from the JSON response
            var hotelData = jsonDocument.RootElement.GetProperty("data");

            // Construct a HotelDetails object from the JSON data
            var hotelDetails = new HotelDetails
            {
                HotelName = hotelData.GetProperty("hotel_name").GetString(), // Hotel name
                Address = hotelData.GetProperty("address").GetString(), // Hotel address
                City = hotelData.GetProperty("city").GetString(), // City where the hotel is located
                CountryCode = hotelData.GetProperty("countrycode").GetString(), // Country code
                AvailableRooms = hotelData.GetProperty("available_rooms").GetInt32(), // Number of available rooms
                                                                                      // Parse the average room size and format it to 2 decimal places, handle conversion failure
                AverageRoomSize = double.TryParse(hotelData.GetProperty("average_room_size_for_ufi_m2").GetString(), out double roomSize)
                                  ? roomSize.ToString("F2") // Convert to string with 2 decimal places
                                  : "N/A", // Return "N/A" if parsing fails
                Url = hotelData.GetProperty("url").GetString() // URL for the hotel details
            };

            return hotelDetails; // Return the constructed HotelDetails object
        }

        // Asynchronous method to retrieve room availability for a given hotel and date range
        public async Task<RoomAvailability> GetRoomAvailabilityAsync(int hotelId, DateTime minDate, DateTime maxDate)
        {
            // Format the minimum and maximum dates to the required format (e.g., yyyy-MM-dd)
            string min_date = minDate.ToString("yyyy-MM-dd"); // Format minimum date
            string max_date = maxDate.ToString("yyyy-MM-dd"); // Format maximum date

            // Construct the API URL with the required query parameters for room availability
            var response = await _httpClient.GetAsync($"https://booking-com15.p.rapidapi.com/api/v1/hotels/getAvailability?hotel_id={hotelId}&min_date={min_date}&max_date={max_date}&currency_code=USD");
            response.EnsureSuccessStatusCode(); // Ensure the request was successful

            // Read the JSON response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();
            // Parse the JSON response
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Access the main 'data' object from the JSON response
            var availabilityData = jsonDocument.RootElement.GetProperty("data");

            // Construct a RoomAvailability object to hold the availability data
            var roomAvailability = new RoomAvailability
            {
                Currency = availabilityData.GetProperty("currency").GetString(), // Currency for the prices
                AvDates = new Dictionary<string, decimal>() // Initialise the dictionary to hold available dates and prices
            };

            // Parse avDates as an array of objects
            foreach (var dateEntry in availabilityData.GetProperty("avDates").EnumerateArray())
            {
                // Enumerate through each date object
                foreach (var date in dateEntry.EnumerateObject())
                {
                    roomAvailability.AvDates[date.Name] = date.Value.GetDecimal(); // Store the date and its corresponding price
                }
            }

            // Parse lengthsOfStay as count of days from the availability data
            roomAvailability.LengthsOfStay = availabilityData.GetProperty("lengthsOfStay").GetArrayLength();

            return roomAvailability; // Return the constructed RoomAvailability object
        }

        public async Task<List<HotelReview>> GetHotelReviewsAsync(int hotelId)
        {
            // Construct the URL for fetching hotel reviews based on the provided hotel ID
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelReviews?hotel_id={hotelId}&sort_option_id=sort_most_relevant&page_number=1&languagecode=en-us";

            // Send a GET request to the specified URL
            var response = await _httpClient.GetAsync(url);

            // Ensure the response indicates success; otherwise, an exception will be thrown
            response.EnsureSuccessStatusCode();

            // Read the content of the response as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to create a JsonDocument for further processing
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Initialise a list to hold the hotel reviews
            var reviewsList = new List<HotelReview>();

            // Iterate through each review in the JSON data
            foreach (var reviewJson in jsonDocument.RootElement.GetProperty("data").GetProperty("result").EnumerateArray())
            {
                // Create a new HotelReview object and populate its properties from the JSON data
                var review = new HotelReview
                {
                    Title = reviewJson.GetProperty("title").GetString(), // Extract the title of the review
                    Pros = reviewJson.GetProperty("pros").GetString(), // Extract positive feedback
                    Cons = reviewJson.GetProperty("cons").GetString(), // Extract negative feedback
                    Date = reviewJson.GetProperty("date").GetString(), // Extract the date of the review
                    AverageScore = (int)reviewJson.GetProperty("average_score").GetDouble(), // Extract the average score and cast it to int
                    TravelPurpose = reviewJson.GetProperty("travel_purpose").GetString() // Extract the travel purpose for the review
                };

                // Add the review to the list of reviews
                reviewsList.Add(review);
            }

            // Return the list of reviews after processing
            return reviewsList;
        }

        public async Task<List<HotelPhoto>> GetHotelPhotosAsync(int hotelId)
        {
            // Construct the URL for fetching hotel photos based on the provided hotel ID
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelPhotos?hotel_id={hotelId}&languagecode=en-us";

            // Send a GET request to the specified URL
            var response = await _httpClient.GetAsync(url);

            // Ensure the response indicates success; otherwise, an exception will be thrown
            response.EnsureSuccessStatusCode();

            // Read the content of the response as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to create a JsonDocument for further processing
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Initialise a list to hold the hotel photos
            var photosList = new List<HotelPhoto>();

            // Iterate through each photo in the JSON data
            foreach (var photoJson in jsonDocument.RootElement.GetProperty("data").EnumerateArray())
            {
                // Create a new HotelPhoto object and populate its properties from the JSON data
                var photo = new HotelPhoto
                {
                    Id = photoJson.GetProperty("id").GetInt32(), // Extract the photo ID
                    Url = photoJson.GetProperty("url").GetString() // Extract the photo URL
                };

                // Add the photo to the list of photos
                photosList.Add(photo);
            }

            // Return the list of photos after processing
            return photosList;
        }

        public async Task<List<FlightOffer>> SearchFlightsAsync(string fromId, string toId, DateTime departDate, DateTime returnDate)
        {
            // Construct the URL for searching flights with the provided parameters
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/flights/searchFlights?fromId={fromId}&toId={toId}&departDate={departDate:yyyy-MM-dd}&returnDate={returnDate:yyyy-MM-dd}&pageNo=1&adults=1&children=0,17&sort=BEST&cabinClass=ECONOMY&currency_code=AED";

            // Send a GET request to the specified URL to search for flights
            var response = await _httpClient.GetAsync(url);

            // Ensure the response indicates success; otherwise, throw an exception
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to create a JsonDocument for processing
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Initialise a list to hold flight offers
            var flightOffersList = new List<FlightOffer>();

            // Check if the 'data' property exists in the JSON response
            if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
            {
                // Check if the 'aggregation' property exists within 'data'
                if (dataElement.TryGetProperty("aggregation", out var aggregationElement))
                {
                    // Iterate through each airline in the 'airlines' array
                    foreach (var airline in aggregationElement.GetProperty("airlines").EnumerateArray())
                    {
                        // Create a new FlightOffer object and populate its properties from the JSON data
                        var offer = new FlightOffer
                        {
                            Key = airline.GetProperty("iataCode").GetString(), // Extract IATA code of the airline
                            Price = airline.GetProperty("minPrice").GetProperty("units").GetInt32(), // Extract minimum price
                            Currency = airline.GetProperty("minPrice").GetProperty("currencyCode").GetString(), // Extract currency code
                            DurationMin = aggregationElement.GetProperty("durationMin").GetInt32(), // Extract minimum duration
                            DurationMax = aggregationElement.GetProperty("durationMax").GetInt32() // Extract maximum duration
                        };

                        // Add the flight offer to the list
                        flightOffersList.Add(offer);
                    }
                }
            }

            // Return the list of flight offers after processing
            return flightOffersList;
        }

        public async Task<List<TaxiOffer>> SearchTaxiAsync(string pickUpPlaceId, string dropOffPlaceId, DateTime pickUpDate, string pickUpTime)
        {
            // Construct the URL for searching taxi offers with the provided parameters
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/taxi/searchTaxi?pick_up_place_id={pickUpPlaceId}&drop_off_place_id={dropOffPlaceId}&pick_up_date={pickUpDate:yyyy-MM-dd}&pick_up_time={pickUpTime}&currency_code=EUR";

            // Send a GET request to the specified URL to search for taxi offers
            var response = await _httpClient.GetAsync(url);

            // Ensure the response indicates success; otherwise, throw an exception
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to create a JsonDocument for processing
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Initialise a list to hold taxi offers
            var taxiOffersList = new List<TaxiOffer>();

            // Check if the 'data' property exists in the JSON response
            if (jsonDocument.RootElement.TryGetProperty("data", out var dataElement))
            {
                // Check if the 'results' property exists within 'data'
                if (dataElement.TryGetProperty("results", out var resultsElement))
                {
                    // Iterate through each result in the 'results' array
                    foreach (var result in resultsElement.EnumerateArray())
                    {
                        // Create a new TaxiOffer object and populate its properties from the JSON data
                        var offer = new TaxiOffer
                        {
                            ResultId = result.GetProperty("resultId").GetString(), // Extract the result ID
                            VehicleType = result.GetProperty("vehicleType").GetString(), // Extract vehicle type
                            Description = result.GetProperty("descriptionLocalised").GetString(), // Extract localised description
                            SupplierName = result.GetProperty("supplierName").GetString(), // Extract supplier name
                            PassengerCapacity = result.GetProperty("passengerCapacity").GetInt32(), // Extract passenger capacity
                            Duration = result.GetProperty("duration").GetInt32(), // Extract duration of the trip
                            ImageUrl = result.GetProperty("imageUrl").GetString(), // Extract image URL
                            MeetGreet = result.GetProperty("meetGreet").GetBoolean(), // Extract meet and greet option
                                                                                      // Get price details with default value handling
                            PriceAmount = result.TryGetProperty("price", out var priceElement)
                                          ? decimal.Parse(priceElement.GetProperty("amount").GetString())
                                          : 0, // Default value if price is not found
                            Currency = result.TryGetProperty("price", out var priceElement2)
                                          ? priceElement2.GetProperty("currencyCode").GetString()
                                          : string.Empty, // Default value if currency is not found
                            NonRefundable = result.GetProperty("nonRefundable").GetBoolean(), // Extract non-refundable status
                            DrivingDistance = result.GetProperty("drivingDistance").GetDecimal(), // Extract driving distance
                        };

                        // Add the taxi offer to the list
                        taxiOffersList.Add(offer);
                    }
                }
            }

            // Return the list of taxi offers after processing
            return taxiOffersList;
        }

        public async Task<List<SearchHotels>> SearchHotelDetailsAsync(int destId, string searchType, string arrivalDate, string departureDate)
        {
            // Construct the GET request to search for hotel details with the specified parameters
            var response = await _httpClient.GetAsync($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={destId}&search_type={searchType}&arrival_date={arrivalDate}&departure_date={departureDate}&page_number=1&units=metric&temperature_unit=c&languagecode=en-us&currency_code=AED");

            // Ensure the response indicates success; otherwise, throw an exception
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to create a JsonDocument for processing
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            // Initialise a list to hold hotel details
            var hotelDetailsList = new List<SearchHotels>();

            // Check if the 'data' property exists in the JSON response
            if (jsonDocument.RootElement.TryGetProperty("data", out JsonElement dataElement) && dataElement.TryGetProperty("hotels", out JsonElement hotelsArray))
            {
                // Iterate through each hotel in the 'hotels' array
                foreach (var hotelJson in hotelsArray.EnumerateArray())
                {
                    // Create a new SearchHotels object and populate its properties from the JSON data
                    var hotelDetails = new SearchHotels
                    {
                        HotelId = hotelJson.GetProperty("hotel_id").GetInt32(), // Extract hotel ID
                        Name = CleanUpString(hotelJson.GetProperty("property").TryGetProperty("name", out JsonElement nameElement) ? nameElement.GetString() : string.Empty), // Extract and clean up hotel name
                        AccessibilityLabel = CleanUpString(hotelJson.TryGetProperty("accessibilityLabel", out JsonElement accessibilityElement) ? accessibilityElement.GetString() : string.Empty), // Extract and clean up accessibility label
                        Price = hotelJson.GetProperty("property").TryGetProperty("priceBreakdown", out JsonElement priceBreakdownElement) && priceBreakdownElement.TryGetProperty("grossPrice", out JsonElement grossPriceElement) ? grossPriceElement.GetProperty("value").GetDouble() : 0, // Extract gross price
                        Currency = hotelJson.GetProperty("property").TryGetProperty("priceBreakdown", out JsonElement priceCurrencyElement) && priceCurrencyElement.TryGetProperty("grossPrice", out JsonElement currencyElement) ? currencyElement.GetProperty("currency").GetString() : string.Empty, // Extract currency code
                        Rating = hotelJson.GetProperty("property").TryGetProperty("reviewScore", out JsonElement reviewScoreElement) ? reviewScoreElement.GetDouble() : 0, // Extract review score
                        ImageUrl = hotelJson.GetProperty("property").TryGetProperty("photoUrls", out JsonElement photoUrlsElement) && photoUrlsElement.GetArrayLength() > 0 ? photoUrlsElement[0].GetString() : string.Empty, // Extract image URL if available
                        Status = CleanUpString(hotelJson.TryGetProperty("status", out JsonElement statusElement) ? statusElement.GetString() : string.Empty), // Extract and clean up hotel status
                    };

                    // Add the hotel details to the list
                    hotelDetailsList.Add(hotelDetails);
                }
            }

            // Return the list of hotel details after processing
            return hotelDetailsList;
        }

        // Utility method to clean up unwanted characters from strings
        private string CleanUpString(string input)
        {
            return input.Replace("?", "").Trim(); // Remove '?' and trim whitespace
        }
    }
}