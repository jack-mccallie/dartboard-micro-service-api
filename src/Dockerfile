# Stage 1: Build the .NET Core application on your Apple ARM laptop
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the necessary files to the container
COPY . .

# Build the .NET Core application
RUN dotnet publish -c Release -o out

# Stage 2: Run the .NET Core application on your Raspberry Pi
FROM mcr.microsoft.com/dotnet/runtime:7.0-arm32v7 AS runtime
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/out .

# Expose any necessary ports
EXPOSE 5000

# Set the entry point to run your .NET Core application
ENTRYPOINT ["dotnet", "dartboard-microservice-api.dll"]