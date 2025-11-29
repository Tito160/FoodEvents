using Microsoft.EntityFrameworkCore;

namespace FoodEvents.Biblioteca;

public class FoodEventsDbContext : DbContext
{
    public DbSet<Chef> Chefs => Set<Chef>();
    public DbSet<EventoGastronomico> EventosGastronomicos => Set<EventoGastronomico>();
    public DbSet<Participante> Participantes => Set<Participante>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    public FoodEventsDbContext(DbContextOptions<FoodEventsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Chef>(entity =>
        {
            entity.Property(c => c.NombreCompleto).IsRequired().HasMaxLength(200);
            entity.Property(c => c.EspecialidadCulinaria).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Nacionalidad).IsRequired().HasMaxLength(100);
            entity.Property(c => c.CorreoElectronico).IsRequired().HasMaxLength(200);
            entity.Property(c => c.TelefonoContacto).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<EventoGastronomico>(entity =>
        {
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DescripcionDetallada).IsRequired();
            entity.Property(e => e.Ubicacion).IsRequired().HasMaxLength(300);

            entity.HasOne(e => e.Chef)
                .WithMany(c => c.Eventos)
                .HasForeignKey(e => e.ChefId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Participante>(entity =>
        {
            entity.Property(p => p.NombreCompleto).IsRequired().HasMaxLength(200);
            entity.Property(p => p.CorreoElectronico).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Telefono).IsRequired().HasMaxLength(50);
            entity.Property(p => p.DocumentoIdentidad).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasOne(r => r.Evento)
                .WithMany(e => e.Reservas)
                .HasForeignKey(r => r.EventoGastronomicoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Participante)
                .WithMany(p => p.Reservas)
                .HasForeignKey(r => r.ParticipanteId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
