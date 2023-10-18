# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set build arguments and environment variables
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80 for the application to listen on
EXPOSE 80

# Set the working directory to /src
WORKDIR /src

# Copy the project files for restore
COPY Mc2.CrudTest.Presentation/Server/Mc2.CrudTest.Presentation.Server.csproj Mc2.CrudTest.Presentation/Server/
COPY Configuration/Configurations.csproj Configuration/
COPY Insfrastructure/Infrastructure.csproj Insfrastructure/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Common/Common.csproj Common/
COPY Mc2.CrudTest.Presentation/Client/Mc2.CrudTest.Presentation.Client.csproj Mc2.CrudTest.Presentation/Client/
COPY Mc2.CrudTest.Presentation/Shared/Mc2.CrudTest.Presentation.Shared.csproj Mc2.CrudTest.Presentation/Shared/

# Restore NuGet packages for the projects
RUN dotnet restore "Mc2.CrudTest.Presentation/Server/Mc2.CrudTest.Presentation.Server.csproj"
RUN dotnet restore "Configuration/Configurations.csproj"
RUN dotnet restore "Insfrastructure/Infrastructure.csproj"
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Common/Common.csproj"
RUN dotnet restore "Mc2.CrudTest.Presentation/Client/Mc2.CrudTest.Presentation.Client.csproj"
RUN dotnet restore "Mc2.CrudTest.Presentation/Shared/Mc2.CrudTest.Presentation.Shared.csproj"

# Copy the rest of the application source code
COPY . .

# Build the server project in Release mode
RUN dotnet build "Mc2.CrudTest.Presentation/Server/Mc2.CrudTest.Presentation.Server.csproj" -c Release -o /app/build

# Set the entry point for running the server
ENTRYPOINT ["dotnet", "/app/build/Mc2.CrudTest.Presentation.Server.dll"]

# Specify command-line arguments for the server
CMD ["--server.urls", "http://0.0.0.0:80"]
