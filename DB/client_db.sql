--
-- PostgreSQL database cluster dump
--

-- Started on 2026-09-17 11:49:41 -05

\restrict SlCrwcjcjhibtyD8UPQs9AK1yB4PR1a0cOEccsdylndkGDFePxBLNa8bmz0L8t0

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








\unrestrict SlCrwcjcjhibtyD8UPQs9AK1yB4PR1a0cOEccsdylndkGDFePxBLNa8bmz0L8t0

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

\restrict Y8ywGLi38g293xAQm8pSl34KKz2K0oM9QjdgACoD7nogROIUyaCTjByFPJxGWLJ

-- Dumped from database version 16.15
-- Dumped by pg_dump version 18.3

-- Started on 2026-09-17 11:49:41 -05

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

-- Completed on 2026-09-17 11:49:41 -05

--
-- PostgreSQL database dump complete
--

\unrestrict Y8ywGLi38g293xAQm8pSl34KKz2K0oM9QjdgACoD7nogROIUyaCTjByFPJxGWLJ

--
-- Database "client_db" dump
--

--
-- PostgreSQL database dump
--

\restrict TvytDMRA1z3Ho7wZH6Q60Q8t6mwgq4sZNvVvE3va8PMTrMufgHFXPQQIB9TPIgD

-- Dumped from database version 16.15
-- Dumped by pg_dump version 18.3

-- Started on 2026-09-17 11:49:41 -05

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
-- TOC entry 3460 (class 1262 OID 16384)
-- Name: client_db; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE client_db WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE client_db OWNER TO postgres;

\unrestrict TvytDMRA1z3Ho7wZH6Q60Q8t6mwgq4sZNvVvE3va8PMTrMufgHFXPQQIB9TPIgD
\connect client_db
\restrict TvytDMRA1z3Ho7wZH6Q60Q8t6mwgq4sZNvVvE3va8PMTrMufgHFXPQQIB9TPIgD

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
-- TOC entry 216 (class 1259 OID 16394)
-- Name: Clientes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Clientes" (
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
    "UpdatedAt" timestamp with time zone
);


ALTER TABLE public."Clientes" OWNER TO postgres;

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
-- TOC entry 3454 (class 0 OID 16394)
-- Dependencies: 216
-- Data for Name: Clientes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Clientes" ("Id", "ContrasenaHash", "Estado", "Nombre", "Genero", "Edad", "Identificacion", "Direccion", "Telefono", "CreatedAt", "DeletedAt", "UpdatedAt") FROM stdin;
5451644c-ef56-41ba-8133-0e5b9f56cc57	$2a$11$8NrJDJ1LzTGsaGDWCr4rreWLScGM6IM.UHjtL5K3E3XVQNATvdFYO	t	Juan Perez	Masculino	30	1234567890	Calle 1	0999999999	2026-09-16 14:59:52.210723+00	\N	\N
794ce8d3-0884-4dfb-8e64-060615555da2	$2a$11$duHVR/RQfXWN9PFN/17WxugLelsvyDEjjmkA0C.epN.hz0Zr0KzxS	t	Fernando	Masculino	26	1751652289	El Inca	0962978920	2026-09-16 23:08:34.764479+00	2026-09-16 23:13:06.930845+00	2026-09-16 23:13:06.93103+00
af6e6ce1-edd4-40e2-8481-7a1fc9dc3305	$2a$11$vVjmZ0plejc0NTu0a/FGwuK4LjOp5Rc8iMXtFLsN1FDj1A1OO29Re	t	Ana Lopez	Femenino	28	1789602714	Calle X	099999	2026-09-16 23:51:54.638356+00	\N	\N
\.


--
-- TOC entry 3453 (class 0 OID 16389)
-- Dependencies: 215
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20260916041815_InitialCreate	10.0.12
20260916052654_AddAuditColumns	10.0.12
20260916234734_RemoveClienteId	10.0.12
\.


--
-- TOC entry 3309 (class 2606 OID 16401)
-- Name: Clientes PK_Clientes; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Clientes"
    ADD CONSTRAINT "PK_Clientes" PRIMARY KEY ("Id");


--
-- TOC entry 3307 (class 2606 OID 16393)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


-- Completed on 2026-09-17 11:49:41 -05

--
-- PostgreSQL database dump complete
--

\unrestrict TvytDMRA1z3Ho7wZH6Q60Q8t6mwgq4sZNvVvE3va8PMTrMufgHFXPQQIB9TPIgD

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

\restrict cvCyhYb5LX0Rtwk4TUz1Wbg3bVavmXbe1ydXZ2zve9FPqhaIqohhepKKEUTz7Qj

-- Dumped from database version 16.15
-- Dumped by pg_dump version 18.3

-- Started on 2026-09-17 11:49:41 -05

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

-- Completed on 2026-09-17 11:49:41 -05

--
-- PostgreSQL database dump complete
--

\unrestrict cvCyhYb5LX0Rtwk4TUz1Wbg3bVavmXbe1ydXZ2zve9FPqhaIqohhepKKEUTz7Qj

-- Completed on 2026-09-17 11:49:41 -05

--
-- PostgreSQL database cluster dump complete
--

