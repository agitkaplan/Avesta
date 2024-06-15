using Avesta.Core.Cryptography;
using Avesta.Core.System;
using Microsoft.Extensions.Configuration;

namespace Avesta.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<CryptographyService>();

            builder.Services.AddControllers();

            builder.Services.Configure<CoreOptions>(builder.Configuration.GetSection(CoreOptions.Key));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
