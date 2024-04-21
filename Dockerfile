# Use the Microsoft SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SaturdayQuizWeb/SaturdayQuizWeb.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY SaturdayQuizWeb/. ./
RUN dotnet build -c Release -o out

# Publish the app
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SaturdayQuizWeb.dll"]
