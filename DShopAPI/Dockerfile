# Use the official .NET Core SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image as the base image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the built application from the build image
COPY --from=build /app/out .

# Expose the port on which the API will listen
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "DShopAPI.dll"]
