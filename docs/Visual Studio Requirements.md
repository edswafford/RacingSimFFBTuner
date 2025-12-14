Visual Studio Solution Requirements

**Project Templates & NuGet Recommendations**

**Essential NuGet packages**

* Microsoft.Extensions.Hosting
* Microsoft.Extensions.DependencyInjection
* Microsoft.Extensions.DependencyInjection.Abstractions
* Microsoft.Extensions.Logging
* Microsoft.Extensions.Logging.Abstractions
* Microsoft.NET.Test.Sdk
* xUnit
* xunit.runner.visualstudio
* Moq
* System.Threading.Channels (lock-free channels for high-throughput queues)
* Microsoft.Extensions.Configuration
* Microsoft.Extensions.Configuration.Json
* Microsoft.Extensions.Configuration.EnvironmentVariables
* CommunityToolkit.Mvvm for lightweight MVVM helpers
* BenchmarkDotNet for microbenchmarks of FFB code

**Directory / Solution Skeleton (Prototype)**

RacingSimFFB.sln

Namespace RacingSimFFB

src/

Domain/

Common/

Constants/

Entities/

Enums/

Events/

Exceptions/

Shared/

ValueObjects/

Interfaces/

Domain.csproj

GlobalUsings.cs

Application/

Features/

UseCases/

Interfaces/ (e.g., for external services like email, logging)

Application.csproj

GlobalUsings.cs

Infrastructure/

Services/

Persistence/

Logging/

Application.csproj

GlobalUsings.cs

Presentation/

Dialogs/

Models/

View/

ViewModels/

App.xaml

App.xaml.cs

MainWindow.xaml

MainWindow.xaml.cs

tests/

UnitTests/

IntegrationTests/

docs/

architecture.md

telemetry-protocols.md

tools/

sample-data/ # telemetry captures for tests

.gitignore

README.md