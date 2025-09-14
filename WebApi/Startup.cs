namespace WebApi;

using Abstraction.IRepositories;
using Abstraction.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // CORS configuration
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["JwtSettings:Issuer"],
                        ValidAudience = Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"]))
                    };
                });

        // SQL Server configuration
        services.AddDbContext<Data.Data.TradeMarketDbContext>(options =>
            options.UseSqlServer(this.Configuration.GetConnectionString("Market")));
        services.AddScoped<IUnitOfWork, Data.Data.UnitOfWork>();

        //var mongoClient = new MongoClient(mongoConnectionString);
        services.AddScoped<IProductService, Business.Services.ProductService>();
        services.AddScoped<ICustomerService, Business.Services.CustomerService>();
        services.AddScoped<IReceiptService, Business.Services.ReceiptService>();
        services.AddScoped<IStatisticService, Business.Services.StatisticService>();
        services.AddScoped<IUserService, Business.Services.UserService>();

        services.AddAutoMapper(typeof(Business.AutomapperProfile).Assembly);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trade Market API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trade Market API v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        // Use CORS
        app.UseCors("AllowAnyOrigin");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
