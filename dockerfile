# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Install the missing library
RUN apt-get update && apt-get install -y libkrb5-dev

# Copy only the project file(s) first to leverage Docker's layer caching
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BillingService.dll"]