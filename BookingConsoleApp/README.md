BookingConsoleApp

Project Overview
The BookingConsoleApp is a console application designed to facilitate the booking of hotels, taxis, and flights. It integrates with the Booking.com API to provide users with real-time information about available accommodations, taxi offers, and flight details. The application aims to enhance the user experience by offering a seamless interface for travel-related services.

Folder Structure
BookingConsoleApp
│
├── References
│
├── Models
│   ├── HotelData.cs
│   ├── HotelDetails.cs
│   ├── HotelPhoto.cs
│   ├── HotelReview.cs
│   ├── RoomAvailability.cs
│   ├── SearchFlights.cs
│   └── SearchTaxi.cs
│
├── Services
│   └── BookingService.cs
│
├── App.config
├── appsettings.json
├── packages.config
└── Program.cs
├── Recorded Data
│   ├── Payment_records.txt
│   ├── Error_Logs.txt

Description of Each Component
Models:
	HotelData.cs: Contains the data structure for hotel-related information.
	HotelDetails.cs: Holds detailed information about specific hotels.
	HotelPhoto.cs: Represents the structure for hotel images.
	HotelReview.cs: Contains customer review data for hotels.
	HotelSearch.cs: Allows the user to search for hotels.
	RoomAvailability.cs: Represents room availability information for hotels.
	SearchFlights.cs: Defines the structure for flight search results.
	SearchTaxi.cs: Defines the structure for taxi search results.

Services:
	BookingService.cs: Contains the business logic for interacting with the Booking.com API, handling searches for hotels, taxis, and flights.

Configuration Files:
	App.config: Contains application-level configuration settings.
	appsettings.json: Holds JSON formatted configuration settings for the application.
	packages.config: Lists the NuGet packages used in the project.

Program.cs:
	The entry point of the application, where the main logic and user interface flow are defined.

Payment_records.txt:
	Contains all of the information about what room the customer is booking along with the hotelID, what days they intend to stay and their payment information.

Error_logs.txt:
	Contains all the errors that the user has inputted.