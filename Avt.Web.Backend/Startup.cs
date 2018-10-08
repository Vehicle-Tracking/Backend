using System;
using System.Threading;
using AutoMapper;
using Avt.Web.Backend.Data.Base;
using Avt.Web.Backend.Data.Repositories;
using Avt.Web.Backend.Data.Spec;
using Avt.Web.Backend.Service;
using Avt.Web.Backend.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Avt.Web.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        { 
            //services.AddEntityFrameworkInMemoryDatabase()
            //    .AddDbContext<DataContext>((sp, options) =>
            //    {
            //        options.UseInMemoryDatabase("AvtDb")
            //            .UseInternalServiceProvider(sp)
            //            .UseLazyLoadingProxies(false)
            //            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
            //             .EnableSensitiveDataLogging();
            //    });

            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseLazyLoadingProxies(false);
            optionsBuilder.UseSqlite(connection);

            using (var context = new DataContext(optionsBuilder.Options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
            }

            services.AddCors();/*options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                       // .AllowAnyOrigin()
                        .WithOrigins(
                                "http://localhost:4200"
                            )
                        .AllowCredentials();
                }));*/

            services.AddOptions();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });

           // services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            AutoMapper.Mapper.Initialize((mapper) =>
            {
                mapper.AddProfile<MappingProfile>();
            });

            services.AddSignalR(config =>
            {
                config.EnableDetailedErrors = true;
                config.HandshakeTimeout = TimeSpan.FromMinutes(10);
            })/*.AddJsonProtocol(options => {
                options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
            })*/;
            
            services.AddTransient<IDataContext>((ctx) => new DataContext(optionsBuilder.Options));
            services.AddTransient((ctx) => (DbContext)ctx.GetService<IDataContext>());

            services.AddTransient<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IVehicleStatusRepository, VehicleStatusRepository>();
            services.AddTransient<IVehicleOverviewRepository, VehicleOverviewRepository>();
            services.AddTransient<IOwnerRepository, OwnerRepository>();

            services.AddSingleton<PingService>();

            services.AddSingleton<OverviewService>();

            services.AddSingleton<IHostedService, LongRunningWorkerService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            app.UseCors(config =>
                config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());


            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseSignalR(routes =>
            {
                routes.MapHub<PingHub>("/pingHub");
                routes.MapHub<StatusOverviewHub>("/overviewHub");
            });

          //  app.UseWebSockets();

            app.UseMvc();
        }
    }
}
