FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

# Install EF Core CLI tool globally
RUN dotnet tool install --global dotnet-ef --version 9.* \
    && echo "export PATH=\"$PATH:/root/.dotnet/tools\"" >> /root/.bashrc

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

COPY . .

RUN pwd
RUN ls -lrta

RUN dotnet restore
RUN dotnet build --no-restore

ARG FROM_MIGRATION=0
ARG TO_MIGRATION=0

RUN mkdir -p /out 

RUN echo "testin From Migration: "$FROM_MIGRATION
RUN echo "testin To Migration: "$TO_MIGRATION

RUN dotnet ef migrations script $FROM_MIGRATION $TO_MIGRATION \
      --project orders.database \
      --startup-project orders.webapi \
      --context OrderDbContext \
      --idempotent \
      --output /out/migration.sql


FROM scratch AS export-stage
COPY --from=builder /out/migration.sql .
