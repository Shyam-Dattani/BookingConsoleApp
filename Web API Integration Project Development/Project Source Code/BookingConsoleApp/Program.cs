using Microsoft.Extensions.Configuration; // For configuration management
using System;
using System.IO; // For file handling
using System.Threading.Tasks; // For asynchronous programming
using BookingConsoleApp.Services; // For booking service operations
using System.Collections.Generic; // For collection management
using BookingConsoleApp.Models; // For model definitions
using System.Globalization; // For culture-specific operations
using System.Linq; // For LINQ methods like All
using System.Text.RegularExpressions; // For Regex

namespace BookingConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration from appsettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set base path to the current directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load configuration settings
                .Build();

            // Retrieve API key from configuration
            string apiKey = configuration["ApiSettings:ApiKey"];

            // Initialise booking service with the retrieved API key
            var bookingService = new BookingService(apiKey);

            // Display a welcome message to the user
            Console.WriteLine("Welcome to AceBooking - Travel and Accommodation Booking System");

            bool continueBooking = true; // Variable to control the menu loop

            string directoryPath = @"D:\Users\Shyam Dattani\Desktop\Shyam Dattani_P2785659 & Gavin Bisla_ P2796362_AceBooking\BookingConsoleApp\Recorded Data"; // Change this as necessary
            Directory.CreateDirectory(directoryPath); // Ensure the directory exists

            // Create the error log file path
            string errorLogPath = Path.Combine(directoryPath, "error_logs.txt");
            // Create the error log file if it doesn't exist
            if (!File.Exists(errorLogPath))
            {
                File.Create(errorLogPath).Dispose(); // Create the file and close it immediately
            }

            while (continueBooking) // Start a loop that continues until the user opts to exit
            {
                // Display the main menu options to the user
                Console.WriteLine("\nPlease select an option:");
                Console.WriteLine("1. Search Hotel Destination");
                Console.WriteLine("2. Search Hotel Details");
                Console.WriteLine("3. Get Hotel Details");
                Console.WriteLine("4. Get Room Availability");
                Console.WriteLine("5. Get Hotel Reviews");
                Console.WriteLine("6. Get Hotel Photos");
                Console.WriteLine("7. Search Flights");
                Console.WriteLine("8. Search Taxi");
                Console.WriteLine("9. Exit");

                // Prompt the user to enter their choice from the menu
                Console.Write("Enter your choice (1-9): ");
                string choice = Console.ReadLine(); // Read user input

                if (choice == "1")
                {
                    Console.Write("Enter a destination: ");
                    string destination = Console.ReadLine(); // Read the user input for the destination

                    try
                    {
                        List<HotelData> hotels = await bookingService.SearchHotelsAsync(destination);

                        if (hotels.Count > 0) // Check if any hotels were found
                        {
                            Console.WriteLine($"Hotels in {destination}:");
                            foreach (var hotel in hotels) // Iterate through the list of hotels
                            {
                                // Display hotel details
                                Console.WriteLine($"Destination ID: {hotel.DestId}");
                                Console.WriteLine($"Name: {hotel.Name}");
                                Console.WriteLine($"Region: {hotel.Region}");
                                Console.WriteLine($"Country: {hotel.Country}");
                                Console.WriteLine($"Latitude: {hotel.Latitude}, Longitude: {hotel.Longitude}");
                                Console.WriteLine($"Image URL: {hotel.ImageUrl}");
                                Console.WriteLine($"-------------------------------"); // Separator for clarity
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No hotels found in {destination}.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(errorLogPath, ex.Message, $"Destination: {destination}"); // Log the error
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();

                }
                else if (choice == "2")
                {
                    string destinationInputs = ""; // Initialise to capture user inputs
                    try
                    {
                        Console.Write("Enter destination ID: ");
                        int destId = int.Parse(Console.ReadLine());
                        destinationInputs += $"Destination ID: {destId}, ";

                        Console.Write("Enter search type (e.g., City): ");
                        string searchType = Console.ReadLine();
                        destinationInputs += $"Search Type: {searchType}, ";

                        Console.Write("Enter arrival date (DD-MM-YYYY): ");
                        string arrivalDateInput = Console.ReadLine();
                        destinationInputs += $"Arrival Date: {arrivalDateInput}, ";

                        Console.Write("Enter departure date (DD-MM-YYYY): ");
                        string departureDateInput = Console.ReadLine();
                        destinationInputs += $"Departure Date: {departureDateInput}, ";

                        // Convert dates from DD-MM-YYYY to YYYY-MM-DD
                        DateTime arrivalDate = DateTime.ParseExact(arrivalDateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime departureDate = DateTime.ParseExact(departureDateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                        // Format dates as YYYY-MM-DD for API
                        string formattedArrivalDate = arrivalDate.ToString("yyyy-MM-dd");
                        string formattedDepartureDate = departureDate.ToString("yyyy-MM-dd");

                        List<SearchHotels> hotelDetails = await bookingService.SearchHotelDetailsAsync(destId, searchType, formattedArrivalDate, formattedDepartureDate);

                        if (hotelDetails.Count > 0)
                        {
                            Console.WriteLine($"Hotels found:");
                            foreach (var hotel in hotelDetails)
                            {
                                Console.WriteLine($"Hotel ID: {hotel.HotelId}");
                                Console.WriteLine($"Name: {hotel.Name}");
                                Console.WriteLine($"Accessibility Label: {hotel.AccessibilityLabel}");
                                Console.WriteLine($"Price: {hotel.Price} {hotel.Currency}");
                                Console.WriteLine($"Rating: {hotel.Rating}");
                                Console.WriteLine($"Image URL: {hotel.ImageUrl}");
                                Console.WriteLine($"-------------------------------");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No hotels found.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        LogError(errorLogPath, ex.Message, destinationInputs); // Log the error
                        Console.WriteLine("Input format is incorrect. Please ensure dates are in DD-MM-YYYY format.");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        LogError(errorLogPath, ex.Message, destinationInputs); // Log the error
                        Console.WriteLine("The date entered is out of range. Please enter a valid date.");
                    }
                    catch (Exception ex)
                    {
                        LogError(errorLogPath, ex.Message, destinationInputs); // Log the error
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "3")
                {
                    // Get hotel ID from user
                    Console.Write("Enter hotel ID: ");
                    if (int.TryParse(Console.ReadLine(), out int hotelId)) // Attempt to parse user input as an integer
                    {
                        // Get arrival and departure dates from user
                        Console.Write("Enter arrival date (DD-MM-YYYY): ");
                        DateTime arrivalDate;
                        // Validate the arrival date format
                        while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out arrivalDate))
                        {
                            Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                        }

                        Console.Write("Enter departure date (DD-MM-YYYY): ");
                        DateTime departureDate;
                        // Validate the departure date format
                        while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out departureDate))
                        {
                            Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                        }

                        try
                        {
                            // Fetch hotel details using the hotel ID and dates
                            HotelDetails hotelDetails = await bookingService.GetHotelDetailsAsync(hotelId, arrivalDate, departureDate);

                            // Check if hotelDetails is null
                            if (hotelDetails != null) // If hotel details were retrieved successfully
                            {
                                // Output the hotel details
                                Console.WriteLine($"Hotel Name: {hotelDetails.HotelName}");
                                Console.WriteLine($"Address: {hotelDetails.Address}");
                                Console.WriteLine($"City: {hotelDetails.City}");
                                Console.WriteLine($"Country: {hotelDetails.CountryCode}");
                                Console.WriteLine($"Available Rooms: {hotelDetails.AvailableRooms}");
                                Console.WriteLine($"Average Room Size: {hotelDetails.AverageRoomSize} m²");
                                Console.WriteLine($"Hotel Link: {hotelDetails.Url}");
                            }
                            else
                            {
                                // Notify user if hotel details were not found
                                Console.WriteLine("Hotel details not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError(errorLogPath, ex.Message, $"Hotel ID: {hotelId}, Arrival Date: {arrivalDate.ToString("dd-MM-yyyy")}, Departure Date: {departureDate.ToString("dd-MM-yyyy")}"); // Log the error
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid hotel ID.");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "4")
                {
                    // Get hotel ID from user
                    Console.Write("Enter hotel ID: ");
                    if (int.TryParse(Console.ReadLine(), out int hotelId)) // Try to parse the hotel ID
                    {
                        // Get min and max dates from user
                        Console.Write("Enter minimum date (DD-MM-YYYY): ");
                        DateTime minDate; // Variable to hold the minimum date
                        while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out minDate)) // Validate date format
                        {
                            Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                        }

                        Console.Write("Enter maximum date (DD-MM-YYYY): ");
                        DateTime maxDate; // Variable to hold the maximum date
                        while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out maxDate)) // Validate date format
                        {
                            Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                        }

                        try
                        {
                            // Fetch room availability
                            RoomAvailability availability = await bookingService.GetRoomAvailabilityAsync(hotelId, minDate, maxDate);

                            // Output the availability
                            Console.WriteLine($"Currency: {availability.Currency}"); // Display the currency
                            foreach (var date in availability.AvDates) // Loop through available dates
                            {
                                Console.WriteLine($"Date: {date.Key}, Price: {date.Value} {availability.Currency}"); // Display date and price
                            }

                            // Process payment
                            ProcessPayment(hotelId, minDate.ToString("yyyy-MM-dd"), maxDate.ToString("yyyy-MM-dd"), directoryPath); // Call the payment processing method
                        }
                        catch (Exception ex)
                        {
                            LogError(errorLogPath, ex.Message, $"Hotel ID: {hotelId}, Min Date: {minDate.ToString("dd-MM-yyyy")}, Max Date: {maxDate.ToString("dd-MM-yyyy")}"); // Log the error
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid hotel ID.");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "5")
                {
                    // Get hotel ID from the user
                    Console.Write("Enter hotel ID: ");
                    if (int.TryParse(Console.ReadLine(), out int hotelId)) // Try to parse the hotel ID
                    {
                        try
                        {
                            // Fetch hotel reviews
                            List<HotelReview> reviews = await bookingService.GetHotelReviewsAsync(hotelId);

                            // Output the reviews
                            if (reviews.Count > 0) // Check if there are any reviews
                            {
                                Console.WriteLine("Hotel Reviews:");
                                foreach (var review in reviews) // Loop through each review
                                {
                                    Console.WriteLine($"Title: {review.Title}"); // Display the review title
                                    Console.WriteLine($"Pros: {review.Pros}"); // Display positive aspects
                                    Console.WriteLine($"Cons: {review.Cons}"); // Display negative aspects
                                    Console.WriteLine($"Date: {review.Date:yyyy-MM-dd}"); // Display the review date
                                    Console.WriteLine($"Rating: {review.AverageScore}/5"); // Display the review rating
                                    Console.WriteLine($"Purpose: {review.TravelPurpose}"); // Display the purpose of travel
                                    Console.WriteLine(new string('-', 30)); // Separator for readability
                                }
                            }
                            else
                            {
                                // Notify user if no reviews are available
                                Console.WriteLine("No reviews available for this hotel.");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError(errorLogPath, ex.Message, $"Hotel ID: {hotelId}"); // Log the error
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid hotel ID.");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "6")
                {
                    // Get hotel ID from the user
                    Console.Write("Enter hotel ID: ");
                    if (int.TryParse(Console.ReadLine(), out int hotelId)) // Attempt to parse the hotel ID
                    {
                        try
                        {
                            // Fetch hotel photos
                            List<HotelPhoto> photos = await bookingService.GetHotelPhotosAsync(hotelId);

                            // Output the photos
                            if (photos.Count > 0) // Check if any photos were returned
                            {
                                Console.WriteLine("Hotel Photos:");
                                foreach (var photo in photos) // Loop through each photo
                                {
                                    Console.WriteLine($"ID: {photo.Id}, URL: {photo.Url}"); // Display photo ID and URL
                                }
                            }
                            else
                            {
                                // Notify user if no photos are available
                                Console.WriteLine("No photos available for this hotel.");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError(errorLogPath, ex.Message, $"Hotel ID: {hotelId}"); // Log the error
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid hotel ID.");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "7")
                {
                    Console.Write("Enter departure location ID: ");
                    string fromId = Console.ReadLine(); // Read the departure location ID

                    Console.Write("Enter arrival location ID: ");
                    string toId = Console.ReadLine(); // Read the arrival location ID

                    Console.Write("Enter departure date (DD-MM-YYYY): ");
                    DateTime departDate;
                    while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out departDate))
                    {
                        Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                    }

                    Console.Write("Enter return date (DD-MM-YYYY): ");
                    DateTime returnDate;
                    while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out returnDate))
                    {
                        Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                    }

                    try
                    {
                        Console.WriteLine("Searching for flight offers, please wait...");
                        List<FlightOffer> flightOffers = await bookingService.SearchFlightsAsync(fromId, toId, departDate, returnDate);

                        if (flightOffers.Count > 0)
                        {
                            Console.WriteLine("Flight Offers:");
                            foreach (var offer in flightOffers)
                            {
                                Console.WriteLine($"Key: {offer.Key}");
                                Console.WriteLine($"Price: {offer.Price} {offer.Currency}");
                                Console.WriteLine($"Duration (Min - Max): {offer.DurationMin} - {offer.DurationMax} hours");
                                Console.WriteLine(new string('-', 30));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No flight offers found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(errorLogPath, ex.Message, $"From ID: {fromId}, To ID: {toId}, Departure Date: {departDate.ToString("dd-MM-yyyy")}, Return Date: {returnDate.ToString("dd-MM-yyyy")}"); // Log the error
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "8")
                {
                    Console.Write("Enter pick-up location ID: ");
                    string pickUpPlaceId = Console.ReadLine(); // Read the pick-up location ID

                    Console.Write("Enter drop-off location ID: ");
                    string dropOffPlaceId = Console.ReadLine(); // Read the drop-off location ID

                    Console.Write("Enter pick-up date (DD-MM-YYYY): ");
                    DateTime pickUpDate;
                    while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out pickUpDate))
                    {
                        Console.WriteLine("Invalid date format. Please enter again (DD-MM-YYYY):");
                    }

                    Console.Write("Enter pick-up time (HH:MM in 24-hour format): ");
                    string pickUpTime = Console.ReadLine(); // Read the pick-up time

                    try
                    {
                        Console.WriteLine("Searching for taxi offers, please wait...");
                        List<TaxiOffer> taxiOffers = await bookingService.SearchTaxiAsync(pickUpPlaceId, dropOffPlaceId, pickUpDate, pickUpTime);

                        if (taxiOffers.Count > 0)
                        {
                            Console.WriteLine("Taxi Offers:");
                            foreach (var offer in taxiOffers)
                            {
                                Console.WriteLine($"Result ID: {offer.ResultId}");
                                Console.WriteLine($"Vehicle Type: {offer.VehicleType}");
                                Console.WriteLine($"Description: {offer.Description}");
                                Console.WriteLine($"Supplier Name: {offer.SupplierName}");
                                Console.WriteLine($"Passenger Capacity: {offer.PassengerCapacity}");
                                Console.WriteLine($"Price: {offer.PriceAmount} {offer.Currency}");
                                Console.WriteLine($"Duration: {offer.Duration} minutes");
                                Console.WriteLine($"Image URL: {offer.ImageUrl}");
                                Console.WriteLine($"Meet & Greet: {offer.MeetGreet}");
                                Console.WriteLine(new string('-', 30));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No taxi offers found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(errorLogPath, ex.Message, $"Pick-up Place ID: {pickUpPlaceId}, Drop-off Place ID: {dropOffPlaceId}, Pick-up Date: {pickUpDate.ToString("dd-MM-yyyy")}, Pick-up Time: {pickUpTime}"); // Log the error
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                }
                else if (choice == "9")
                {
                    continueBooking = false; // Set the loop control variable to false to exit
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }

            Console.WriteLine("Press any key to exit..."); // Prompt user to exit
            Console.ReadKey(); // Wait for a key press before closing the application
        }

        private static void LogError(string errorLogPath, string errorMessage, string userInputs)
        {
            string logEntry = $"Timestamp: {DateTime.UtcNow}; Error: {errorMessage}; User Inputs: {userInputs}\n";
            File.AppendAllText(errorLogPath, logEntry);
        }

        static void ProcessPayment(int hotelId, string startDate, string endDate, string directoryPath)
        {
            string cardNumber;
            string expiryDate;
            string cvv;

            // Validate card number with retries
            while (true)
            {
                Console.Write("Enter card number: ");
                cardNumber = Console.ReadLine(); // Capture the card number

                if (cardNumber.Length == 16 && cardNumber.All(char.IsDigit))
                {
                    break; // Exit loop if valid
                }
                Console.WriteLine("Invalid card number. Please enter exactly 16 digits.");
            }

            // Validate expiry date with retries
            while (true)
            {
                Console.Write("Enter expiry date (MM/YY): ");
                expiryDate = Console.ReadLine(); // Capture the card's expiry date

                if (Regex.IsMatch(expiryDate, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                {
                    break; // Exit loop if valid
                }
                Console.WriteLine("Invalid expiry date format. Please use MM/YY.");
            }

            // Validate CVV with retries
            while (true)
            {
                Console.Write("Enter CVV: ");
                cvv = Console.ReadLine(); // Capture the CVV code

                if (cvv.Length == 3 && cvv.All(char.IsDigit))
                {
                    break; // Exit loop if valid
                }
                Console.WriteLine("Invalid CVV. Please enter exactly 3 digits.");
            }

            // Specify the file path for recording payment data
            string paymentRecordPath = Path.Combine(directoryPath, "payment_records.txt");

            // Prepare the record string with payment details
            string record = $"Hotel ID: {hotelId}, Start Date: {startDate}, End Date: {endDate}, Card Number: {cardNumber}, Expiry Date: {expiryDate}, CVV: {cvv}\n";

            // Append the record to the payment records file
            File.AppendAllText(paymentRecordPath, record);  // Synchronous method

            // Notify the user that the payment has been processed
            Console.WriteLine("Payment processed and recorded.");
            Console.WriteLine("Thank you for Booking with Ace Booking"); // Thank the user for booking
        }
    }
}