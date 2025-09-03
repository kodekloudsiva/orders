#!/bin/bash
set -e

echo "migration entry point, pwd: " && pwd

echo "migration entry point, ls: " && ls -lrta

echo "📦 Ensuring database [$DB_NAME] exists..."
/opt/mssql-tools/bin/sqlcmd -S $DB_HOST -U $DB_USER -P $DB_PASS -d master -Q "IF DB_ID('$DB_NAME') IS NULL CREATE DATABASE [$DB_NAME];"

echo "📜 Applying migrations from migration.sql ..."
/opt/mssql-tools/bin/sqlcmd -S $DB_HOST -U $DB_USER -P $DB_PASS -d $DB_NAME -i migration.sql

echo "✅ Migration applied successfully."
