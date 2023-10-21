using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Timers;

namespace MagicVilla_API.Datos
{
    public class VillaContext:DbContext
    {
        public VillaContext(DbContextOptions<VillaContext> options) :base(options)
        {
            
        }

        public DbSet<Villa> Villas {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Esta encima de los balsos",
                    ImagenUrl = "",
                    Ocupantes = 7,
                    MetrosCuadrados=50,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Vista a la playa",
                    Detalle = "Desde el balcon del Hotel se ve playa",
                    ImagenUrl = "",
                    Ocupantes = 30,
                    MetrosCuadrados = 100,
                    Tarifa = 120,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }
            );
        }
    }
}
