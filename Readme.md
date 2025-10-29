# Prague Parking V2

A simple .NET 8 console application for managing a parking garage, built with C# 12 and Spectre.Console for interactive UI.

## Features

- **Park Vehicle:** Add a car or motorcycle to the garage by entering its type and license plate.
- **Remove Vehicle:** Remove a vehicle by license plate and view checkout info.
- **Move Vehicle:** Move a parked vehicle to another spot, checking for available space.
- **Show Garage Map:** Visual display of all parking spots and parked vehicles.
- **Optimize Motorcycles:** Rearrange motorcycles for optimal space usage.
- **Show/Change Prices:** View and update parking prices for each vehicle type.
- **Resize Garage:** Change the number of parking spots without losing vehicles (as long as space allows).
- **Save/Load Data:** Configuration and garage data are saved and loaded automatically.

## Configuration

- The configuration and data files are stored in the `/bin/debug/NET8.0` directory relative to the executable.
    - `appconfig.json` — vehicle types, sizes, and prices.
    - `parking_garage_data.json` — current garage state.

## Getting Started

1. Build and run the project in Visual Studio 2022.
2. Follow the interactive menu to manage your parking garage.

## Requirements

- .NET 8 SDK
- Visual Studio 2022

## Repository

[GitHub Repository](https://github.com/PavlosKats/Prague_Park_v2)
