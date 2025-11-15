
using CarRentalApp.Configuration;
using CarRentalApp.Data;
using CarRentalApp.Data.Repository;
using CarRentalApp.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CarRentalApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            #region Configure Entity Framework
            // Configure Entity Framework and SQL Server
            builder.Services.AddDbContext<CarRentalDBContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("CarRentalDb"))
            );
            #endregion

            #region Configuration for Identity Server and Authentication
            // Configuration for Identity Server and Authentication can be added here
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<CarRentalDBContext>()
                    .AddDefaultTokenProviders();
            #endregion

            #region configuration for JWT
            // Key for JWT
            var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("LocalScretKey")!);
            // Local issure and audience for JWT
            var LocalIssuer = builder.Configuration.GetValue<string>("LocalIssuer");
            var LocalAudience = builder.Configuration.GetValue<string>("LocalAudience");
            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("AuthForLocal", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = LocalIssuer,
                    ValidateAudience = true,
                    ValidAudience = LocalAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,


                };
            });

            #endregion
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region swaggaer Configuration
            builder.Services.AddSwaggerGen(options=>
            {
                options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
                {
                    Description = "JWT authorization header using the bearer scheme." +
                    " Enter bearer [space] add your token in the text input. Example: Bearer #kdeu^*^#@DARER",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                // Adding security requirement
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id ="Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Scheme = "oauth2",
                            Name ="Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
            #endregion

            // Auto Mapper Configurations
            builder.Services.AddAutoMapper(typeof(AutoMapperConfi));

            #region Dependency Injection
            // Dependency Injection for repository
            builder.Services.AddScoped(typeof(ICarRentalRepository<>), typeof(CarRentalRepository<>));
            builder.Services.AddScoped<IFileStorageService, FileStorageService>();
            // Dependency Injection for services
            builder.Services.AddScoped<IAuthUserService, AuthUserService>();
            builder.Services.AddScoped<ICarsService, CarsService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<IFavouriteCarsService, FavouriteCarsService>();
            builder.Services.AddScoped<IFavouriteRespository, FavouriteRespository>();
            #endregion

            #region Adding cores policy for angular application
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4209")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            #endregion
            var app = builder.Build();
            app.UseStaticFiles();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            // ? CORS should come BEFORE Authentication/Authorization

            app.UseCors("AllowAngularApp");

            app.UseAuthentication();  
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
