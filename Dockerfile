
# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY  . .
RUN dotnet restore

# Copy the rest of the source code and build it
COPY . .
RUN dotnet publish -c Release -o out

# Use the smaller runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port (match your app settings)
EXPOSE 5000

# Run the app
ENTRYPOINT ["dotnet", "UrlShortner.Api.dll"]

