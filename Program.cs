using Blazored.LocalStorage;
using GestionGastos.DataContext;
using GestionGastosShared.Services;
using GestionGastosShared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Microsoft.OpenApi.Models;
using System.Text;


public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Obtiene la configuraci�n directamente desde el builder
        var configuration = builder.Configuration;
        string cadenaConexion = configuration.GetConnectionString("mysqlremoto");

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();

        // Configuraci�n de DbContext
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


        //builder.Services.AddScoped<GastosService>();
        //builder.Services.AddScoped<CategoriasService>();
        //builder.Services.AddScoped<IUsuariosService, UsuariosService>();
        //builder.Services.AddScoped<IAuthService, AuthService>();
        //builder.Services.AddScoped<TokenService>();


        // Agregar Swagger para la documentaci�n de la API
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        // Configurar autenticaci�n JWT
        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        options.RequireHttpsMetadata = false;
        //        options.SaveToken = true;
        //        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("clave_super_secreta")), // Reemplaza con tu clave real
        //            ValidateIssuer = false,
        //            ValidateAudience = false
        //        };
        //    });

        // Agregar Swagger con soporte para autenticaci�n JWT
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });

            // Definir el esquema de seguridad JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Introduce el token en el siguiente formato: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            // Agregar requisito de seguridad global
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
        });


        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  builder.WithOrigins("https://ggw.azurewebsites.net") // Cambia por el puerto de tu frontend
                                  //builder.WithOrigins("https://localhost:7076/")
                                         .AllowAnyHeader()
                                         .AllowAnyMethod();
                              });
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

        //No es necesario usar autenticaci�n ni autorizaci�n
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
