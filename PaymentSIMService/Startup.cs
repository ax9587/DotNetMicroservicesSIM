using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PaymentSIMService.Data;
using PaymentSIMService.Init;
using PaymentSIMService.Jobs;
using PaymentSIMService.Messaging.EventProcessing;
using PaymentSIMService.Messaging.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentSIMService
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
            services.AddDbContext<HangfireContext>(options =>
                    options.UseNpgsql(Configuration.GetSection("BackgroundJobs").Get<BackgroundJobsConfig>().HangfireConnectionStringName));
            services.AddDbContext<PaymentContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("PaymentConn")));
            services.AddPaymentDemoInitializer();
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddScoped<IPolicyAccountRepository, PolicyAccountRepository>();
            services.AddHostedService<MessageBusSubscriber>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentSIMService", Version = "v1" });
            });
            
            services.AddBackgroundJobs(Configuration.GetSection("BackgroundJobs").Get<BackgroundJobsConfig>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HangfireContext hfcontext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentSIMService v1"));
            }

            //app.UseHttpsRedirection();

            hfcontext.Database.EnsureCreated();

            app.UseCorsMiddleware();  //TODO

            app.UseRouting();

            app.UseAuthorization();

            app.UseInitializer();  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseBackgroundJobs();  //todo Npgsql.PostgresException: '3D000: database "hangfireJobs" does not exist'  Create Database Missing
        }
    }
}
