# FoodEvents

Sistema para gestionar eventos gastronómicos (eventos, chefs/organizadores, participantes y reservas) desarrollado en C# con ASP.NET Core Web API, biblioteca de dominio y Entity Framework Core sobre MySQL (Pomelo).

## Proyectos

- **foodEvents.Biblioteca**: capa de dominio y aplicación.
  - Entidades: `Chef`, `EventoGastronomico`, `Participante`, `Reserva`.
  - Enumeraciones: `TipoEventoGastronomico`, `MetodoPago`, `EstadoReserva`.
  - `FoodEventsDbContext` para acceso a datos con EF Core + MySQL.
  - `ValidadorDominio`: validación centralizada de reglas de negocio (emails, teléfonos, fechas, capacidad de eventos, etc.).
  - `FoodEventsService`: servicios de aplicación usados por la Web API.

- **foodEvents.WebApi**: capa de presentación HTTP (ASP.NET Core Web API).
  - Configura el `FoodEventsDbContext` contra MySQL.
  - Expone controladores:
    - `ChefsController`
    - `EventosController`
    - `ParticipantesController`
    - `ReservasController`

- **foodEvents.Tests**: tests automatizados (xUnit).
  - Pruebas básicas de validación de dominio (`ValidadorDominio`).

## Configuración de base de datos

En `foodEvents.WebApi/appsettings.json` configurar la cadena de conexión:

- Servidor MySQL (`server`, `port`).
- Base de datos (`database`).
- Usuario y contraseña (`user`, `password`).

Ejemplo (modificar `TU_PASSWORD`):

```json
"ConnectionStrings": {
  "FoodEventsDb": "server=localhost;port=3306;database=FoodEventsDb;user=root;password=TU_PASSWORD;TreatTinyAsBoolean=true;"
}
```

## Configuración y ejecución del proyecto

### 1. Prerrequisitos

- **.NET 8.0 SDK** instalado
- **MySQL Server** (versión 8.0 o superior) instalado y en ejecución
- **IDE** (Visual Studio, VS Code, Rider)

### 2. Configuración de MySQL

1. Asegúrate de que MySQL Server esté instalado y corriendo
2. Crea la base de datos (opcional - si no existe, se creará automáticamente):
   ```sql
   CREATE DATABASE FoodEventsDb;
   ```
3. Configura la cadena de conexión en `foodEvents.WebApi/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "FoodEventsDb": "server=localhost;port=3306;database=FoodEventsDb;user=root;password=TU_PASSWORD;TreatTinyAsBoolean=true;"
   }
   ```
   - Reemplaza `TU_PASSWORD` con tu contraseña de MySQL
   - Ajusta `server` y `port` si tu MySQL no está en localhost:3306
   - El usuario `root` puede ser reemplazado por otro usuario con permisos

### 3. Ejecutar el proyecto

Desde la terminal, en la raíz del proyecto:

```bash
# Restaurar paquetes NuGet
dotnet restore

# Compilar la solución
dotnet build

# Ejecutar la API Web
dotnet run --project foodEvents.WebApi
```

La API se iniciará automáticamente en:
- **URL**: `https://localhost:7123` y `http://localhost:5123`
- **Swagger UI**: `https://localhost:7123/swagger`

### 4. Probar la API con Swagger

1. Abre tu navegador y navega a `https://localhost:7123/swagger`
2. Verás la documentación interactiva de todos los endpoints
3. Puedes probar cada endpoint directamente desde Swagger:
   - **GET /api/chefs**: Listar todos los chefs
   - **POST /api/chefs**: Crear un nuevo chef
   - **GET /api/eventos**: Listar todos los eventos
   - **POST /api/eventos**: Crear un nuevo evento
   - **GET /api/participantes**: Listar todos los participantes
   - **POST /api/participantes**: Crear un nuevo participante
   - **GET /api/reservas**: Listar todas las reservas
   - **POST /api/reservas**: Crear una nueva reserva

### 5. Notas importantes

- **Base de datos**: Se crea automáticamente con `EnsureCreated()` al iniciar la API
- **Validación**: Todos los datos se validan antes de guardar (emails, teléfonos, fechas, capacidad)
- **Respuestas JSON**: La API usa DTOs para evitar ciclos de referencia y respuestas limpias
- **HTTPS**: Swagger redirige automáticamente a HTTPS; acepta el certificado autofirmado del navegador

### 6. Ejemplos de uso en Swagger

**Crear un Chef**:
```json
{
  "nombreCompleto": "Gordon Ramsay",
  "email": "gordon@restaurant.com",
  "telefono": "1234567890",
  "especialidad": "Cocina Francesa"
}
```

**Crear un Evento**:
```json
{
  "nombre": "Festival Gastronómico 2024",
  "descripcion": "Evento de alta cocina",
  "fechaInicio": "2024-12-25T18:00:00",
  "fechaFin": "2024-12-25T23:00:00",
  "ubicacion": "Centro de Convenciones",
  "capacidadMaxima": 100,
  "tipoEvento": 0,
  "precioEntrada": 150.00,
  "chefId": 1
}
```

**Crear una Reserva**:
```json
{
  "eventoId": 1,
  "participanteId": 1,
  "cantidadPersonas": 2,
  "metodoPago": 0
}
```

## Validación centralizada

Toda la lógica de validación de negocio se concentra en `ValidadorDominio` dentro de la biblioteca, por ejemplo:

- Formato de correos electrónicos.
- Teléfonos numéricos y de longitud lógica.
- Coherencia de fechas de eventos (fin no puede ser antes que inicio).
- Capacidad de eventos al crear reservas (no se permiten reservas en eventos llenos).

Los controladores Web API llaman a `FoodEventsService`, que a su vez utiliza `ValidadorDominio` y devuelve mensajes de error claros cuando los datos son inválidos.

## Arquitectura del proyecto

- **foodEvents.Biblioteca**: Capa de dominio y aplicación
  - `Entities/`: Clases de dominio (Chef, EventoGastronomico, Participante, Reserva)
  - `Enums/`: Enumeraciones (TipoEventoGastronomico, MetodoPago, EstadoReserva)
  - `Validation/`: ValidadorDominio y ResultadoValidacion
  - `Persistence/`: FoodEventsDbContext (EF Core)
  - `Services/`: FoodEventsService (lógica de aplicación)
  - `Common/`: Clases comunes (ResultadoOperacion)

- **foodEvents.WebApi**: Capa de presentación HTTP
  - `Controllers/`: Controladores API
  - `Dtos/`: DTOs para respuestas JSON limpias
  - `Program.cs`: Configuración y startup

- **foodEvents.Tests**: Pruebas unitarias (xUnit)
