using Microsoft.EntityFrameworkCore;
using Examenes.API.Data;
using System.Text.Json.Serialization;

namespace ExamenAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ExamenApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            builder.Services.AddControllers()
                .AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Configuración estándar para documentación con Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Habilita Swagger solo en desarrollo para probar los endpoints visualmente
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Usar la política de CORS antes de la autorización
            app.UseCors("AllowAll");

            app.UseHttpsRedirection(); // Redirige tráfico HTTP a HTTPS
            app.UseAuthorization(); // Habilita el middleware de autorización

            app.MapControllers(); // Mapea las rutas de los controladores (ej: api/Examenes)

            app.Run(); // Inicia la aplicación
        }
    }
}