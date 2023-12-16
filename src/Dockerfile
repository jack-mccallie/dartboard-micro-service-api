# Use the official .NET 7 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the entire project and restore/build the app
COPY . .

# Build the runtime image
WORKDIR /app
RUN dotnet build

# Expose the port your app runs on
EXPOSE 8080

# Command to run the application
ENTRYPOINT ["dotnet", "run"]