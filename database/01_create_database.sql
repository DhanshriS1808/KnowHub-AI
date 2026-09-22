-- KnowHub AI - database provisioning
--
-- Run once as a superuser (postgres):
--   psql -U postgres -v app_password='<choose-a-password>' -f database/01_create_database.sql
--
-- Creates the application role and database. Schema itself is owned by
-- EF Core migrations (see backend/KnowHub/KnowHub.Infrastructure/Migrations).

\set ON_ERROR_STOP on

-- Application role: no superuser, no createdb. Owns the knowhub database only.
SELECT format('CREATE ROLE knowhub_app LOGIN PASSWORD %L', :'app_password')
WHERE NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'knowhub_app')
\gexec

SELECT 'CREATE DATABASE knowhub OWNER knowhub_app ENCODING ''UTF8'''
WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'knowhub')
\gexec

-- Keep the public schema from being writable by every role (PG15+ default is
-- already restrictive; this makes the intent explicit).
\connect knowhub
REVOKE CREATE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO knowhub_app;
