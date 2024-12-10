using GestionGastos.DataContext;
using GestionGastosShared.Services;
using GestionGastosShared.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;


public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Obtiene la configuración directamente desde el builder
        var configuration = builder.Configuration;
        string cadenaConexion = configuration.GetConnectionString("mysqlremoto");

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddHttpClient();

        // Configuración de DbContext
        builder.Services.AddDbContext<GestionGastosContext>(options =>
            options.UseMySql(cadenaConexion, ServerVersion.AutoDetect(cadenaConexion),
            options => options.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
            ));


        //builder.Services.AddScoped(sp =>
        //{
        //    var handler = new HttpClientHandler
        //    {
        //        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        //    };
        //    return new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
        //});



        builder.Services.AddScoped<IGastosService, GastosService>();
        builder.Services.AddScoped<ICategoriasService, CategoriasService>();
        builder.Services.AddScoped<IUsuariosService, UsuariosService>();


        // Agregar Swagger para la documentación de la API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  builder.WithOrigins("https://localhost:7076") // Cambia por el puerto de tu frontend
                                         .AllowAnyHeader()
                                         .AllowAnyMethod();
                              });
        });

        var app = builder.Build();

        app.UseCors(MyAllowSpecificOrigins);


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // No es necesario usar autenticación ni autorización
        // app.UseAuthentication(); 
        // app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
