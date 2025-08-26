FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

# Install EF Core CLI tool globally
RUN dotnet tool install --global dotnet-ef --version 8.* \
    && echo "export PATH=\"$PATH:/root/.dotnet/tools\"" >> /root/.bashrc

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet build --no-restore

ARG FROM_MIGRATION
ARG TO_MIGRATION
ARG OUTPUT_SCRIPT=sql-scripts/migration.sql

RUN mkdir -p sql-scripts && \
    dotnet ef migrations script $FROM_MIGRATION $TO_MIGRATION \
      --project orders.database \
      --startup-project orders.api \
      --context OrderDbContext \
      --idempotent \
      -o $OUTPUT_SCRIPT
