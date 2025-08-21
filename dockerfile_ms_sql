# Use Microsoft SQL Server 2022 base image
FROM mcr.microsoft.com/mssql/server:2022-latest

# Accept environment variables from outside
ENV SA_PASSWORD=${SA_PASSWORD} #SA_PASSWORD Environment variable (set at runtime for security)
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Developer

# Install required packages
RUN apt-get update && \
    apt-get install -y curl gnupg2 apt-transport-https unixodbc-dev && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 && \
    ln -sfn /opt/mssql-tools18/bin/sqlcmd /usr/bin/sqlcmd && \
    ln -sfn /opt/mssql-tools18/bin/bcp /usr/bin/bcp && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

EXPOSE 1433

ENTRYPOINT ["/opt/mssql/bin/sqlservr"]