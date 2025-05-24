# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY MCComputersBackend/*.csproj ./MCComputersBackend/
WORKDIR /app/MCComputersBackend
RUN dotnet restore

# Copy the rest of the application code
WORKDIR /app
COPY MCComputersBackend/ ./MCComputersBackend/

# Build the application
WORKDIR /app/MCComputersBackend
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "MCComputersBackend.dll"]
