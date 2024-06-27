# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file
COPY ScoreOracleCSharp.sln ./

# Copy the project file(s)
COPY src/ScoreOracleCSharp/ScoreOracleCSharp.csproj ./src/ScoreOracleCSharp/

# Debug: List contents of /app to verify paths
RUN ls -la /app
RUN ls -la /app/src/ScoreOracleCSharp

# Restore any dependencies
RUN dotnet restore

# Copy the rest of the application source code and build the application
COPY src/ ./src/
WORKDIR /app/src/ScoreOracleCSharp
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "ScoreOracleCSharp.dll"]
