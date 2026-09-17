-- ======================================================================
-- Script: BaseDatos.sql
-- Descripción: Creación de esquemas, entidades y datos semilla
-- Motores: PostgreSQL (Microservicios Client y Account)
-- ======================================================================

-- ----------------------------------------------------------------------
-- 1. BASE DE DATOS: client_db
-- ----------------------------------------------------------------------
CREATE DATABASE client_db;
\c client_db;

CREATE TABLE "Clientes" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "ContrasenaHash" character varying(500) NOT NULL,
    "Estado" boolean NOT NULL,
    "Nombre" character varying(200) NOT NULL,
    "Genero" character varying(20) NOT NULL,
    "Edad" integer NOT NULL,
    "Identificacion" character varying(20) NOT NULL,
    "Direccion" character varying(300) NOT NULL,
    "Telefono" character varying(20) NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Clientes" PRIMARY KEY ("Id")
);

-- Datos de prueba (Clientes)
INSERT INTO "Clientes" ("Id", "ContrasenaHash", "Estado", "Nombre", "Genero", "Edad", "Identificacion", "Direccion", "Telefono", "CreatedAt") 
VALUES 
('5451644c-ef56-41ba-8133-0e5b9f56cc57', '$2a$11$8NrJDJ1LzTGsaGDWCr4rreWLScGM6IM.UHjtL5K3E3XVQNATvdFYO', true, 'Juan Perez', 'Masculino', 30, '1234567890', 'Calle 1', '0999999999', '2026-09-16 14:59:52.210723+00'),
('af6e6ce1-edd4-40e2-8481-7a1fc9dc3305', '$2a$11$vVjmZ0plejc0NTu0a/FGwuK4LjOp5Rc8iMXtFLsN1FDj1A1OO29Re', true, 'Ana Lopez', 'Femenino', 28, '1789602714', 'Calle X', '099999', '2026-09-16 23:51:54.638356+00');


-- ----------------------------------------------------------------------
-- 2. BASE DE DATOS: account_db
-- ----------------------------------------------------------------------
CREATE DATABASE account_db;
\c account_db;

CREATE TABLE "ClientReplicas" (
    "ClientId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_ClientReplicas" PRIMARY KEY ("ClientId")
);

CREATE TABLE "Cuentas" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "NumeroCuenta" character varying(20) NOT NULL,
    "TipoCuenta" character varying(20) NOT NULL,
    "SaldoInicial" numeric(18,2) NOT NULL,
    "SaldoDisponible" numeric(18,2) NOT NULL,
    "Estado" boolean NOT NULL,
    "ClienteId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Cuentas" PRIMARY KEY ("Id")
);

CREATE TABLE "Movimientos" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "CuentaId" uuid NOT NULL,
    "Fecha" timestamp with time zone NOT NULL,
    "TipoMovimiento" character varying(20) NOT NULL,
    "Valor" numeric(18,2) NOT NULL,
    "Saldo" numeric(18,2) NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone,
    CONSTRAINT "PK_Movimientos" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Movimientos_Cuentas_CuentaId" FOREIGN KEY ("CuentaId") REFERENCES "Cuentas"("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Cuentas_NumeroCuenta" ON "Cuentas" ("NumeroCuenta");
CREATE INDEX "IX_Movimientos_CuentaId" ON "Movimientos" ("CuentaId");

-- Datos de prueba (Cuentas)
INSERT INTO "Cuentas" ("Id", "NumeroCuenta", "TipoCuenta", "SaldoInicial", "SaldoDisponible", "Estado", "ClienteId", "CreatedAt") 
VALUES 
('be45041c-f634-45e2-8b75-b4bee80cf54d', 'final-1789602714', 'Ahorro', 50.00, 50.00, true, 'af6e6ce1-edd4-40e2-8481-7a1fc9dc3305', '2026-09-16 23:51:54.989161+00');