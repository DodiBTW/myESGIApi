FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["myESGIApi.csproj", "."]
COPY ["./Utils/Utils.fsproj", "Utils/"]
# Restore project dependencies
RUN dotnet restore "./myESGIApi.csproj"

# Copy all source files and build the application
COPY . .
WORKDIR "/src/."
RUN dotnet publish "./myESGIApi.csproj" -c $BUILD_CONFIGURATION -o /out /p:UseAppHost=false

# Use the base runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "myESGIApi.dll"]