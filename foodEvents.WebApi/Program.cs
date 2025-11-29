using System.Text.Json.Serialization;
using FoodEvents.Biblioteca;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FoodEventsDb")
    ?? throw new InvalidOperationException("La cadena de conexión 'FoodEventsDb' no está configurada.");

builder.Services.AddDbContext<FoodEventsDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ValidadorDominio>();
builder.Services.AddScoped<FoodEventsService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FoodEventsDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
