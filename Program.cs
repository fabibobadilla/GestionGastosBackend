using GestionGastos.DataContext;
using GestionGastosShared.Services;
using GestionGastosShared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };
        });


        builder.Services.AddScoped<IGastosService, GastosService>();
        builder.Services.AddScoped<ICategoriasService, CategoriasService>();
        builder.Services.AddScoped<IUsuariosService, UsuariosService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // Agregar Swagger para la documentación de la API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Services.AddCors(options =>
        {
            //options.AddPolicy(name: MyAllowSpecificOrigins,
            //                  builder =>
            //                  {
            //                      //builder.WithOrigins("https://ggw.azurewebsites.net") // Cambia por el puerto de tu frontend
            //                      builder.WithOrigins("http://0.0.0.0")
            //                             .AllowAnyHeader()
            //                             .AllowAnyMethod();
            //                  });
            //options.AddPolicy(name: MyAllowSpecificOrigins,
            //    builder =>
            //        {
            //            builder.WithOrigins("http://localhost", "https://localhost", "http://192.168.x.x")
            //               .AllowAnyHeader()
            //               .AllowAnyMethod();
            //        });

            //Para Desarrollo
            options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
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

        //No es necesario usar autenticación ni autorización
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
