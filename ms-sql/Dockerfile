# Use Microsoft SQL Server 2022 base image
FROM mcr.microsoft.com/mssql/server:2022-latest

# Accept environment variables from outside
# SA_PASSWORD Environment variable (set at runtime for security)
ENV SA_PASSWORD=${SA_PASSWORD}
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Developer

# Install dependencies and mssql-tools
RUN apt-get update 

RUN apt-get install -y curl gnupg2 apt-transport-https software-properties-common unixodbc-dev

RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > /etc/apt/trusted.gpg.d/microsoft.gpg 

RUN curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list
    
RUN apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev && \
    ln -sfn /opt/mssql-tools/bin/sqlcmd /usr/bin/sqlcmd && \
    ln -sfn /opt/mssql-tools/bin/bcp /usr/bin/bcp && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

EXPOSE 1433

ENTRYPOINT ["/opt/mssql/bin/sqlservr"]
