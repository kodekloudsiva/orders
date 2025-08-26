FROM mcr.microsoft.com/mssql-tools

WORKDIR /app

COPY sql-scripts/migration.sql .

CMD /opt/mssql-tools/bin/sqlcmd \
  -S $DB_HOST -U $DB_USER -P $DB_PASS -d $DB_NAME \
  -i migration.sql
