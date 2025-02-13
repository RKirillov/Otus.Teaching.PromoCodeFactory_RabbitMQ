using System;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions.Repositories;
using Otus.Teaching.Pcf.ReceivingFromPartner.DataAccess;
using Otus.Teaching.Pcf.ReceivingFromPartner.DataAccess.Data;
using Otus.Teaching.Pcf.ReceivingFromPartner.DataAccess.Repositories;
using Otus.Teaching.Pcf.ReceivingFromPartner.Integration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Otus.Teaching.Pcf.ReceivingFromPartner.Integration.Services;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Abstractions;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core;

namespace Otus.Teaching.Pcf.ReceivingFromPartner.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BusConnectOptions>(Configuration.GetSection(nameof(BusConnectOptions)));

            services.AddControllers().AddMvcOptions(x=> 
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<INotificationGateway, NotificationGateway>();
            services.AddScoped<IDbInitializer, EfDbInitializer>();
            services.AddTransient<IProducerMessageService, ProducerMessageService>();

           services.AddHttpClient<IGivingPromoCodeToCustomerGateway,GivingPromoCodeToCustomerGateway>(c =>
            {
                c.BaseAddress = new Uri(Configuration["IntegrationSettings:GivingToCustomerApiUrl"]);
            });

            /*            services.AddHttpClient<IAdministrationGateway,AdministrationGateway>(c =>
                        {
                            c.BaseAddress = new Uri(Configuration["IntegrationSettings:AdministrationApiUrl"]);
                        });*/

            services.AddDbContext<DataContext>(x =>
            {
                //x.UseSqlite("Filename=PromocodeFactoryReceivingFromPartnerDb.sqlite");
                x.UseNpgsql(Configuration.GetConnectionString("PromocodeFactoryReceivingFromPartnerDb"));
                x.UseSnakeCaseNamingConvention();
                x.UseLazyLoadingProxies();
            });

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory Receiving From Partner API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            dbInitializer.InitializeDb();
        }
    }
}