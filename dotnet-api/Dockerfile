# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN echo 'Hi Siva' && ls -lrt

# Copy only .csproj files first
COPY orders/orders.application/*.csproj orders.orders.application/
COPY orders/orders.domain/*.csproj orders.orders.domain/
COPY orders/orders.infrastructure/*.csproj orders.orders.infrastructure/
COPY orders/orders.shared/*.csproj orders.orders.shared/
COPY orders/orders.database/*.csproj orders.orders.database/
COPY orders/orders.webapi/*.csproj orders.orders.webapi/

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
