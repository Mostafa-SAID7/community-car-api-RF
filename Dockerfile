# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/CommunityCarApi.WebApi/CommunityCarApi.WebApi.csproj", "src/CommunityCarApi.WebApi/"]
COPY ["src/CommunityCarApi.Application/CommunityCarApi.Application.csproj", "src/CommunityCarApi.Application/"]
COPY ["src/CommunityCarApi.Domain/CommunityCarApi.Domain.csproj", "src/CommunityCarApi.Domain/"]
COPY ["src/CommunityCarApi.Infrastructure/CommunityCarApi.Infrastructure.csproj", "src/CommunityCarApi.Infrastructure/"]

RUN dotnet restore "src/CommunityCarApi.WebApi/CommunityCarApi.WebApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/CommunityCarApi.WebApi"
RUN dotnet build "CommunityCarApi.WebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CommunityCarApi.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published files
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "CommunityCarApi.WebApi.dll"]
