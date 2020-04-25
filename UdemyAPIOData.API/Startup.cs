using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UdemyAPIOData.API.Models;

namespace UdemyAPIOData.API
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConStr"]);
            });

            services.AddOData();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ODataConventionModelBuilder();

            //categoriesController
            // [entity set Name]Controller
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Product>("Products");

            // .../odata/categories(1)/totalproductprice
            builder.EntityType<Category>().Action("TotalProductPrice").Returns<int>();

            builder.EntityType<Category>().Collection.Action("TotalProductPrice2").Returns<int>();

            //odata/categories/totalproductprice
            builder.EntityType<Category>().Collection.Action("TotalProductPriceWithParametre").Returns<int>().Parameter<int>("categoryId");

            var actionTotal = builder.EntityType<Category>().Collection.Action("Total").Returns<int>();

            actionTotal.Parameter<int>("a");
            actionTotal.Parameter<int>("b");
            actionTotal.Parameter<int>("c");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // www.api.com/odata/products
                endpoints.Select().Expand().OrderBy().MaxTop(null).Count().Filter();
                endpoints.MapODataRoute("odata", "odata", builder.GetEdmModel());
                endpoints.MapControllers();
            });
        }
    }
}