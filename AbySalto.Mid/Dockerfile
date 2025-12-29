# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first (better layer caching)
COPY AbySalto.Mid/AbySalto.Mid.WebApi.csproj AbySalto.Mid/
COPY AbySalto.Mid.Infrastructure/AbySalto.Mid.Infrastructure.csproj AbySalto.Mid.Infrastructure/
COPY AbySalto.Mid.Application/AbySalto.Mid.Application.csproj AbySalto.Mid.Application/
COPY AbySalto.Mid.Domain/AbySalto.Mid.Domain.csproj AbySalto.Mid.Domain/

RUN dotnet restore AbySalto.Mid/AbySalto.Mid.WebApi.csproj

# Copy the rest and publish
COPY . .
RUN dotnet publish AbySalto.Mid/AbySalto.Mid.WebApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Listen on 8080 inside container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AbySalto.Mid.WebApi.dll"]