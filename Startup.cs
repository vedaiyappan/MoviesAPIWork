using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoviesAPIWork.Filters;
using MoviesAPIWork.Services;

namespace MoviesAPIWork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAPIRequestIO",
                    builder => builder.WithOrigins("https://www.apirequest.io")
                    .WithMethods("GET", "POST").AllowAnyHeader());
            });

            services.AddAutoMapper(typeof(Startup));

            //services.AddTransient<IFileStorageService, AzureStorageService>();

            services.AddTransient<IFileStorageService, InAppStorageService>();
            //services.AddTransient<IHostedService, MovieInTheatersService>(); //

            services.AddHttpContextAccessor();


            //services.AddControllers();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(MyExceptionFilter));
            })
               .AddNewtonsoftJson()
               .AddXmlDataContractSerializerFormatters();

            //services.AddResponseCaching(); //
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            //services.AddSingleton<IRepository, InMemoryRepository>();

            //services.AddTransient<MyActionFilter>();
            //services.AddTransient<IHostedService, WriteToFileHostedService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Use(async (context, next) =>
            //{
            //    using (var swapStream = new MemoryStream())
            //    {
            //        var originalResponseBody = context.Response.Body;
            //        context.Response.Body = swapStream;

            //        await next.Invoke();

            //        swapStream.Seek(0, SeekOrigin.Begin);
            //        string responseBody = new StreamReader(swapStream).ReadToEnd();
            //        swapStream.Seek(0, SeekOrigin.Begin);

            //        await swapStream.CopyToAsync(originalResponseBody);
            //        context.Response.Body = originalResponseBody;

            //        logger.LogInformation(responseBody);
            //    }
            //});

            //app.Map("/map1", (app) =>
            //{
            //    app.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("I'm short-circuiting the pipeline");
            //    });
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseResponseCaching(); //

            app.UseAuthentication(); //


            app.UseAuthorization();

            app.UseCors();

            // This policy would be applied at the Web API level
            //app.UseCors(builder =>
            //builder.WithOrigins("https://www.apirequest.io").WithMethods("GET", "POST").AllowAnyHeader());


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
