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

## Migraciones y base de datos

Para crear la base de datos y las tablas con Entity Framework Core:

1. Instalar las herramientas de EF Core (`dotnet-ef`).
2. Crear la migración inicial apuntando a la biblioteca (`foodEvents.Biblioteca`) y usando la Web API como proyecto de inicio.
3. Aplicar la migración a la base de datos.

## Validación centralizada

Toda la lógica de validación de negocio se concentra en `ValidadorDominio` dentro de la biblioteca, por ejemplo:

- Formato de correos electrónicos.
- Teléfonos numéricos y de longitud lógica.
- Coherencia de fechas de eventos (fin no puede ser antes que inicio).
- Capacidad de eventos al crear reservas (no se permiten reservas en eventos llenos).

Los controladores Web API llaman a `FoodEventsService`, que a su vez utiliza `ValidadorDominio` y devuelve mensajes de error claros cuando los datos son inválidos.
