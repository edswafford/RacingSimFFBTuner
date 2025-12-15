# Racing Sim FFB Tuner

A Windows desktop application that reads real-time telemetry from racing simulators (iRacing, Assetto Corsa, Automobilista, etc.) and outputs high-frequency force feedback (FFB) to wheels (Simucube Pro, Fanatec, etc.).

## Features

- **Telemetry Ingestion**: Reliable real-time telemetry reading from multiple racing simulators via shared memory, UDP, and SDKs
- **Force Feedback Output**: Deterministic FFB output with support for high update rates (360 Hz+)
- **Tuning UI**: User-facing interface for creating and managing FFB profiles
- **Live Visualization**: Real-time telemetry visualization during driving sessions
- **Cross-Adapter Support**: Device abstraction layer supporting multiple wheel manufacturers

## Technology Stack

- **.NET 10.0**: Target framework
- **WPF**: Windows Presentation Foundation for the user interface
- **MVVM**: Model-View-ViewModel architecture pattern
- **Clean Architecture**: Domain, Application, Infrastructure, and Presentation layers
- **Test-Driven Development**: xUnit for unit and integration testing
- **StyleCop Analyzers**: Code style enforcement aligned with IDesign C# Coding Standard v3.01

## Development Environment

- **Primary IDE**: Visual Studio Code (VS Code)
- **Secondary IDE**: Visual Studio 2026 (for building and running)
- **Platform**: Windows only (no cross-platform support)

## Build Requirements

- Visual Studio 2026
- .NET 10.0 SDK
- Windows 10/11

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio 2026 or VS Code
3. Restore NuGet packages
4. Build the solution
5. Run tests to verify the setup

## License

This project is licensed under the GNU General Public License v3.0 (GPL-3.0). See the [LICENSE](LICENSE) file for details.

## Contributing

This is an open-source project. Contributions are welcome! Please ensure all code follows the IDesign C# Coding Standard v3.01 and passes all StyleCop Analyzer checks.

