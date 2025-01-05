# BookingConsoleApp

## Project Overview
The BookingConsoleApp is a console application designed to facilitate the booking of hotels, taxis, and flights. It integrates with the Booking.com API to provide users with real-time information and booking capabilities.

## Features
- **Hotel Booking:** Search and book hotels with real-time availability.
- **Taxi Booking:** Reserve taxis for pickups and drop-offs.
- **Flight Booking:** Search and book flights with real-time data.
- **Customer Reviews:** View reviews for hotels to make informed decisions.
- **Payment Records:** Track and manage booking payments.
- **Error Logging:** Maintain logs of errors encountered during usage.

## Technologies Used
- **C#**: The primary programming language used for the application.
- **.NET Framework**: Provides the development platform for building and running the application.
- **Booking.com API**: Used to fetch real-time data for hotels, flights, and taxis.
- **JSON**: For configuration settings and data exchange.

## How to Run the Project
To get started with the BookingConsoleApp, follow these steps:

1. **Clone the repository:**
    ```
    git clone https://github.com/Shyam-Dattani/BookingConsoleApp.git
    ```
2. **Open the solution** in your preferred IDE, ideally Visual Studio Installer.

3. **Restore the NuGet packages:**
    ```
    dotnet restore
    ```
    
4. **Update the `appsettings.json` and `App.config`** with your Booking.com API credentials and other necessary configuration settings.

5. **Run the application:**
    ```
    dotnet run
    ```
    
## Folder Structure
```
BookingConsoleApp
│
├── Web API Integration Project Design
│   └── Web API Integration Project Design.pdf
├── Web API Integration Project Development
│   ├── Project Source Code
│   │   └── BookingConsoleApp
│   │       ├── Models
│   │       │   ├── HotelData.cs
│   │       │   ├── HotelDetails.cs
│   │       │   ├── HotelPhoto.cs
│   │       │   ├── HotelReview.cs
│   │       │   ├── HotelSearch.cs
│   │       │   ├── RoomAvailability.cs
│   │       │   ├── SearchFlights.cs
│   │       │   └── SearchTaxi.cs
│   │       ├── Properties
│   │       │   └── AssemblyInfo.cs
│   │       ├── Services
│   │       ├── App.config
│   │       ├── BookingConsoleApp.csproj
│   │       ├── Program.cs
│   │       ├── appsettings.json
│   │       ├── packages.config
│   │       ├── Recorded Data
│   │       ├── .gitattributes
│   │       ├── .gitignore
│   │       └── BookingConsoleApp.sln
│   └── Web API Integration Project Development.pdf
├── Web API Integration Project Presentation
│   ├── Demo of AceBooking System.pdf
│   └── Web Integration Project Presentation for Booking.com.pptx
```
## Explanation of Key Folders and Files

### Models:
- **HotelData.cs**: Contains the data structure for hotel-related information.
- **HotelDetails.cs**: Holds detailed information about specific hotels.
- **HotelPhoto.cs**: Represents the structure for hotel images.
- **HotelReview.cs**: Contains customer review data for hotels.
- **HotelSearch.cs**: Allows the user to search for hotels.
- **RoomAvailability.cs**: Represents room availability information for hotels.
- **SearchFlights.cs**: Defines the structure for flight search results.
- **SearchTaxi.cs**: Defines the structure for taxi search results.

### Properties:
- **AssemblyInfo.cs**: Contains metadata about the assembly.

### Services:
- Contains the business logic for interacting with the Booking.com API, handling searches for hotels, taxis, and flights.

### Configuration Files:
- **App.config**: Contains application-level configuration settings.
- **appsettings.json**: Holds JSON formatted configuration settings for the application.
- **packages.config**: Lists the NuGet packages used in the project.

### Program.cs:
The entry point of the application, where the main logic and user interface flow are defined.

### Recorded Data:
Contains recorded payment information and error logs.

### Other Files:
- **.gitattributes**: Defines attributes for pathnames.
- **.gitignore**: Specifies intentionally untracked files to ignore.
- **BookingConsoleApp.csproj**: The project file that defines the project configuration and dependencies.
- **BookingConsoleApp.sln**: The solution file that contains project configurations and build settings.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.
