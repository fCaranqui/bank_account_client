# BankAccount — Technical Assessment (Backend - .NET)

Este repositorio contiene la solución a la prueba técnica para la posición de Backend .NET. El proyecto está diseñado bajo una arquitectura de **microservicios independientes**, donde cada servicio encapsula su propia lógica de negocio y base de datos, comunicándose asincrónicamente mediante eventos.

## 🚀 Alcance del Proyecto (Features Implementados)

El proyecto consta de dos microservicios totalmente independientes (`Client` y `Account`) construidos con **.NET 10**.

*   **Microservicio Client (`/clientes`):** 
    *   Gestión completa (CRUD) de la entidad `Persona` y su herencia `Cliente`.
    *   Uso de TPH (Table-Per-Hierarchy) en Entity Framework Core.
    *   Soft-delete independiente del estado lógico del cliente.
    *   Al crear un cliente exitosamente, emite el evento `ClientCreatedEvent`.
*   **Microservicio Account (`/cuentas`, `/movimientos`, `/reportes`):** 
    *   Gestión de `Cuenta` y registro de `Movimientos` (Depósitos y Retiros).
    *   Validaciones de negocio robustas (ej. control de sobregiros con `SaldoNoDisponibleException`, validación de cuenta inactiva).
    *   Endpoint F4 para generación de estado de cuenta (Reportes) por rango de fechas.
    *   Consume `ClientCreatedEvent` para mantener una réplica técnica (read-model) y validar la existencia de clientes al crear cuentas, manteniendo el aislamiento de los servicios.
*   **Características Técnicas Globales:**
    *   **Arquitectura:** Clean Architecture / DDD (Domain, Application, Infrastructure, Api).
    *   **Base de Datos:** PostgreSQL independiente por microservicio. Migraciones automáticas al inicio en entorno de desarrollo.
    *   **Comunicación Asíncrona:** RabbitMQ + MassTransit para publicar/consumir eventos.
    *   **Envoltorio de Respuestas:** Todas las respuestas de la API utilizan un estándar custom `GatewayResponse<T>`.
    *   **Manejo de Errores:** Middleware global que captura excepciones del dominio y las mapea a los códigos HTTP correctos (400, 401, 404, 409, 500).
    *   **Testing:** Extensas pruebas unitarias (InMemory DB) y pruebas de integración (`WebApplicationFactory`) para ambos servicios.

## 🛠️ Stack Tecnológico

*   **.NET 10** (C# 14)
*   **Entity Framework Core 10** + Npgsql
*   **PostgreSQL 16**
*   **RabbitMQ** + MassTransit
*   **Serilog** (Logs estructurados en consola y archivo)
*   **xUnit** (Testing)
*   **Docker & Docker Compose**

---

## ⚙️ Cómo Levantar el Proyecto

El proyecto está completamente dockerizado. No necesitas instalar bases de datos ni RabbitMQ en tu máquina anfitriona; todo se orquesta a través de Docker Compose.

### Requisitos previos
*   Docker y Docker Compose instalados.

### Pasos de ejecución
1. Abre una terminal en la raíz del repositorio (`BankAccount/`).
2. Ejecuta el siguiente comando para construir y levantar toda la infraestructura (Bases de datos, RabbitMQ y las APIs):
   
```bash
   docker compose up --build
```

3. Espera a que los contenedores reporten un estado "healthy". Las migraciones de base de datos se aplicarán automáticamente al arrancar las APIs.

### Endpoints (Swagger UI)
Una vez iniciados los contenedores, puedes probar y explorar las APIs directamente desde tu navegador:

*   **Client API:** [http://localhost:8080/swagger](http://localhost:8080/swagger)
*   **Account API:** [http://localhost:8081/swagger](http://localhost:8081/swagger)

*(Para detener el proyecto de forma segura y preservar los datos, usa `docker compose down`. Si deseas limpiar la base de datos completamente, usa `docker compose down -v`).*

---

## 🔑 Credenciales (Entorno de Desarrollo)

Dado que este es un ejercicio práctico, las credenciales se incluyen explícitamente para facilitar la revisión del código y la infraestructura local.

**Bases de Datos (PostgreSQL):**
*   **Account DB:**
    *   Host (desde host): `localhost:5433`
    *   Host (interno docker): `account-db:5432`
    *   Usuario: `postgres`
    *   Contraseña: `postgres`
    *   Base de datos: `account_db`
*   **Client DB:**
    *   Host (desde host): `localhost:5434`
    *   Host (interno docker): `client-db:5432`
    *   Usuario: `postgres`
    *   Contraseña: `postgres`
    *   Base de datos: `client_db`

**Mensajería (RabbitMQ):**
*   Panel de Administración (UI): [http://localhost:15672](http://localhost:15672)
*   Conexión AMQP: `localhost:5672`
*   Usuario: `rabbitmq`
*   Contraseña: `rabbitmq`