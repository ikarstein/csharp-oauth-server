using Authlete.Api;
using Authlete.Conf;
using Authlete.Dto;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthorizationServer
{
    public class Program
    {
        static IAuthleteApi CreateAuthleteApi()
        {
            // Create an instance of IAuthleteConfiguration.
            var conf = CreateAuthleteConfiguration();

            // Create an instance of IAuthleteApi.
            return new AuthleteApi(conf);
        }


        static IAuthleteConfiguration CreateAuthleteConfiguration()
        {
            // Load a configuration file and build an instance of
            // IAuthleteConfiguration interface. By default,
            // "authlete.properties" will be loaded. The name of a
            // configuration file can be specified by the
            // environment variable, AUTHLETE_CONFIGURATION_FILE.
            //
            // AuthetePropertiesConfiguration class has three
            // constructors one of which explicitly takes the name
            // of a configuration file.
            //
            // In Authlete.Conf namespace, there exist some other
            // implementations of IAuthleteConfiguration interface.

            return new AuthletePropertiesConfiguration();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IAuthleteApi>(CreateAuthleteApi());
            builder.Services.AddRazorPages();
            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }


    }
}
