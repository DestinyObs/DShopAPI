# Set the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) and restore dependencies
COPY */*.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY DShopAPI/ ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET 6 ASP.NET runtime image as the base image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out ./

# Expose the desired port (replace <PORT_NUMBER> with the actual port number)
EXPOSE 80

# Define the entry point for the container
ENTRYPOINT ["dotnet", "DShopAPI.dll"]
