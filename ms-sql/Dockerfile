# Use Microsoft SQL Server 2022 base image
FROM mcr.microsoft.com/mssql/server:2022-latest

# Accept environment variables from outside
# SA_PASSWORD Environment variable (set at runtime for security)
ENV SA_PASSWORD=${SA_PASSWORD}
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Developer

# Install dependencies and mssql-tools
RUN apt-get update && \
    apt-get install -y curl gnupg2 apt-transport-https software-properties-common unixodbc-dev && \
    mkdir -p /etc/apt/keyrings && \
    curl -sSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /etc/apt/keyrings/microsoft.gpg && \
    curl -sSL https://packages.microsoft.com/config/ubuntu/20.04/prod.list \
      | sed 's|^deb |deb [signed-by=/etc/apt/keyrings/microsoft.gpg] |' \
      > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 && \
    ln -sfn /opt/mssql-tools18/bin/sqlcmd /usr/bin/sqlcmd && \
    ln -sfn /opt/mssql-tools18/bin/bcp /usr/bin/bcp && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

EXPOSE 1433

ENTRYPOINT ["/opt/mssql/bin/sqlservr"]
