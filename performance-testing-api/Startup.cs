using EasyCaching.InMemory;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using performance_testing_api.Controllers;
using performance_testing_api.Data;
using System;
using System.Reflection;

namespace performance_testing_api
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "performance_testing_api", Version = "v1" });
            });


            const string providerName1 = "InMemory1";

            services.AddEFSecondLevelCache(options =>
                    options.UseEasyCachingCoreProvider(providerName1, isHybridCache: false).DisableLogging(true).UseCacheKeyPrefix("EF_")
            );


            services.AddEasyCaching(options =>
            {
                // use memory cache with your own configuration
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = 60,
                        // total count of cache items, default value is 10000
                        SizeLimit = 100,

                        // enable deep clone when reading object from cache or not, default value is true.
                        EnableReadDeepClone = false,
                        // enable deep clone when writing object to cache or not, default value is false.
                        EnableWriteDeepClone = false,
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = 120;
                    // whether enable logging, default is false
                    config.EnableLogging = false;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 5000;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 300;
                }, providerName1);
            });

            var connectionString = "Data Source=DESKTOP-TERE1H0\\SQLEXPRESS;Initial Catalog=Prtest;User Id=sa;Password=#compaq123";

            services.AddDbContextPool<AppDbContext>((serviceProvider, optionsBuilder) =>
                    optionsBuilder
                        .UseSqlServer(
                            connectionString,
                            sqlServerOptionsBuilder =>
                            {
                                sqlServerOptionsBuilder
                                    .CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds)
                                    .EnableRetryOnFailure()
                                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                            })
                        .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));


            //services.AddDbContext<AppDbContext>(c =>
            //{
            //    c.UseSqlServer("Data Source=DESKTOP-TERE1H0\\SQLEXPRESS;Initial Catalog=Prtest;User Id=sa;Password=#compaq123");
            //    c.EnableSensitiveDataLogging(true);
            //    c.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //        .LogTo(Console.WriteLine, LogLevel.Information);
            //});

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<Test>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "performance_testing_api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
