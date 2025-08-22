# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN echo 'Hi Siva' && ls -lrt

# Copy csproj files and restore dependencies
COPY ./orders/orders.application/orders.application.csproj orders.application/
COPY ./orders/orders.domain/orders.domain.csproj orders.domain/
COPY ./orders/orders.infrastructure/orders.infrastructure.csproj orders.infrastructure/
COPY ./orders/orders.shared/orders.shared.csproj orders.shared/
COPY ./orders/orders.webapi/orders.webapi.csproj orders.webapi/

RUN dotnet restore orders.webapi/orders.webapi.csproj

# Copy the rest of the source code
COPY . .

# Publish the application
RUN dotnet publish orders.webapi/orders.webapi.csproj -c Release -o /app/publish /p:UseAppHost=false


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set the URL binding (optional — ensures app binds to port 8080)
ENV ASPNETCORE_URLS=http://+:9090

# Expose the default HTTP port
EXPOSE 9090

# If you use HTTPS, also expose 9091
# EXPOSE 9091

ENTRYPOINT ["dotnet", "orders.webapi.dll"]
