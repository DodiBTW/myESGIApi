
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
namespace MyESGIApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();
            string? SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
            string? Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            string? Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            string? ConnectionString = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            if (SecretKey == null) throw new Exception("JWT_SECRET environment variable not set");
            if (Issuer == null) throw new Exception("JWT_ISSUER environment variable not set");
            if (Audience == null) throw new Exception("JWT_AUDIENCE environment variable not set");
            if (ConnectionString == null) throw new Exception("DEFAULT_CONNECTION environment variable not set");
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging();



            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(SecretKey))
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
