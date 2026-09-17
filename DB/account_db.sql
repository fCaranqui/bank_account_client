--
-- PostgreSQL database cluster dump
--

-- Started on 2026-09-17 11:49:24 -05

\restrict xsgisDWgnobCpa8zYckKnDB8EyBAusDXvV7AXaLtHJbuhd6fLgBa89m50MWweWb

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS;

--
-- User Configurations
--








\unrestrict xsgisDWgnobCpa8zYckKnDB8EyBAusDXvV7AXaLtHJbuhd6fLgBa89m50MWweWb

--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

\restrict vcuGfuweZbL2NYngAbpkvUG8eYKfQLuaOCBRXtlRhcmqzw2OA2iacgNH1DQKKmc

-- Dumped from database version 16.15
-- Dumped by pg_dump version 18.3

-- Started on 2026-09-17 11:49:24 -05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-09-17 11:49:24 -05

--
-- PostgreSQL database dump complete
--

\unrestrict vcuGfuweZbL2NYngAbpkvUG8eYKfQLuaOCBRXtlRhcmqzw2OA2iacgNH1DQKKmc

--
-- Database "account_db" dump
--

--
-- PostgreSQL database dump
--

\restrict QAiTBekPW1hIyGbArFwTqR3gsyW3vQyhXyVfxhcD9VFkfahViw0OQvoDgqzrEEt

-- Dumped from database version 16.15
-- Dumped by pg_dump version 18.3

-- Started on 2026-09-17 11:49:24 -05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3479 (class 1262 OID 16384)
-- Name: account_db; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE account_db WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE account_db OWNER TO postgres;

\unrestrict QAiTBekPW1hIyGbArFwTqR3gsyW3vQyhXyVfxhcD9VFkfahViw0OQvoDgqzrEEt
\connect account_db
\restrict QAiTBekPW1hIyGbArFwTqR3gsyW3vQyhXyVfxhcD9VFkfahViw0OQvoDgqzrEEt

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16426)
-- Name: ClientReplicas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."ClientReplicas" (
    "ClientId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."ClientReplicas" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 16394)
-- Name: Cuentas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Cuentas" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "NumeroCuenta" character varying(20) NOT NULL,
    "TipoCuenta" character varying(20) NOT NULL,
    "SaldoInicial" numeric(18,2) NOT NULL,
    "SaldoDisponible" numeric(18,2) NOT NULL,
    "Estado" boolean NOT NULL,
    "ClienteId" uuid NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Cuentas" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16400)
-- Name: Movimientos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Movimientos" (
    "Id" uuid DEFAULT gen_random_uuid() NOT NULL,
    "CuentaId" uuid NOT NULL,
    "Fecha" timestamp with time zone NOT NULL,
    "TipoMovimiento" character varying(20) NOT NULL,
    "Valor" numeric(18,2) NOT NULL,
    "Saldo" numeric(18,2) NOT NULL,
    "CreatedAt" timestamp with time zone DEFAULT now() NOT NULL,
    "DeletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Movimientos" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 16389)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 3473 (class 0 OID 16426)
-- Dependencies: 218
-- Data for Name: ClientReplicas; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."ClientReplicas" ("ClientId", "CreatedAt") FROM stdin;
\.


--
-- TOC entry 3471 (class 0 OID 16394)
-- Dependencies: 216
-- Data for Name: Cuentas; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Cuentas" ("Id", "NumeroCuenta", "TipoCuenta", "SaldoInicial", "SaldoDisponible", "Estado", "ClienteId", "CreatedAt", "DeletedAt", "UpdatedAt") FROM stdin;
be45041c-f634-45e2-8b75-b4bee80cf54d	final-1789602714	Ahorro	50.00	50.00	t	af6e6ce1-edd4-40e2-8481-7a1fc9dc3305	2026-09-16 23:51:54.989161+00	\N	\N
\.


--
-- TOC entry 3472 (class 0 OID 16400)
-- Dependencies: 217
-- Data for Name: Movimientos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Movimientos" ("Id", "CuentaId", "Fecha", "TipoMovimiento", "Valor", "Saldo", "CreatedAt", "DeletedAt", "UpdatedAt") FROM stdin;
\.


--
-- TOC entry 3470 (class 0 OID 16389)
-- Dependencies: 215
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20260916041942_InitialCreate	10.0.12
20260916052952_AddAuditColumns	10.0.12
20260916235016_ChangeClienteIdToGuid	10.0.12
20260917155730_AddClientReplica	10.0.12
\.


--
-- TOC entry 3325 (class 2606 OID 16430)
-- Name: ClientReplicas PK_ClientReplicas; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."ClientReplicas"
    ADD CONSTRAINT "PK_ClientReplicas" PRIMARY KEY ("ClientId");


--
-- TOC entry 3320 (class 2606 OID 16399)
-- Name: Cuentas PK_Cuentas; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Cuentas"
    ADD CONSTRAINT "PK_Cuentas" PRIMARY KEY ("Id");


--
-- TOC entry 3323 (class 2606 OID 16405)
-- Name: Movimientos PK_Movimientos; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Movimientos"
    ADD CONSTRAINT "PK_Movimientos" PRIMARY KEY ("Id");


--
-- TOC entry 3317 (class 2606 OID 16393)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3318 (class 1259 OID 16411)
-- Name: IX_Cuentas_NumeroCuenta; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Cuentas_NumeroCuenta" ON public."Cuentas" USING btree ("NumeroCuenta");


--
-- TOC entry 3321 (class 1259 OID 16412)
-- Name: IX_Movimientos_CuentaId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Movimientos_CuentaId" ON public."Movimientos" USING btree ("CuentaId");


--
-- TOC entry 3326 (class 2606 OID 16406)
-- Name: Movimientos FK_Movimientos_Cuentas_CuentaId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Movimientos"
    ADD CONSTRAINT "FK_Movimientos_Cuentas_CuentaId" FOREIGN KEY ("CuentaId") REFERENCES public."Cuentas"("Id") ON DELETE CASCADE;


-- Completed on 2026-09-17 11:49:24 -05

--
-- PostgreSQL database dump complete
--

\unrestrict QAiTBekPW1hIyGbArFwTqR3gsyW3vQyhXyVfxhcD9VFkfahViw0OQvoDgqzrEEt

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

\restrict MJmbFLCDOFEtdAD9RONDr9VXSD5yIJGELpmJiYHGHEkjsG0joEyavLds29ylCr0

-- Dumped from database version 16.15
-- Dumped by pg_dump version 18.3

-- Started on 2026-09-17 11:49:24 -05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-09-17 11:49:24 -05

--
-- PostgreSQL database dump complete
--

\unrestrict MJmbFLCDOFEtdAD9RONDr9VXSD5yIJGELpmJiYHGHEkjsG0joEyavLds29ylCr0

-- Completed on 2026-09-17 11:49:24 -05

--
-- PostgreSQL database cluster dump complete
--

