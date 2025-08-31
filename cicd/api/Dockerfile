# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only .csproj files first
COPY orders.application/ orders.application/
COPY orders.domain/ orders.domain/
COPY orders.infrastructure/ orders.infrastructure/
COPY orders.shared/ orders.shared/
COPY orders.database/ orders.database/
COPY orders.webapi/ orders.webapi/

RUN dotnet restore orders.webapi/orders.webapi.csproj

# Copy the rest of the source code
# COPY . .

# Optional debug to verify files copied (can remove later)
RUN echo "Files in orders.application:" && find orders.application -type f
RUN echo "Files in orders.webapi:" && find orders.webapi -type f

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
