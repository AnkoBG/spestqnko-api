using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Spestqnko.Api.Attributes;
using Spestqnko.Api.Extensions;
using Spestqnko.Api.Settings;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;
using Spestqnko.Data;
using Spestqnko.Data.Repositories;
using Spestqnko.Service;
using System.Text;

namespace Spestqnko.Api.Configurations
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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (connectionString != null)
            {
                services.ConfigureDbContexts(connectionString);
            }
            else
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            // Add MVC controllers with filters
            services.AddControllers(options =>
            {
                options.Filters.Add<AppExceptionFilterAttribute>();
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Register DbContext for repository usage
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<SpestqnkoDbContext>());

            // Register Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IExpenseTresholdRepository, ExpenseTresholdRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserWalletCategoryRepository, UserWalletCategoryRepository>();
            services.AddScoped<IUserWalletRepository, UserWalletRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            
            // Register generic repositories for common entities
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Wallet>, Repository<Wallet>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Expense>, Repository<Expense>>();
            services.AddScoped<IRepository<UserWallet>, Repository<UserWallet>>();
            services.AddScoped<IRepository<UserWalletCategory>, Repository<UserWalletCategory>>();
            services.AddScoped<IRepository<ExpenseTreshold>, Repository<ExpenseTreshold>>();
            services.AddScoped<IRepository<Notification>, Repository<Notification>>();
            services.AddScoped<IRepository<Role>, Repository<Role>>();

            // Register Repository Manager
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            
            // Register services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWalletService, WalletService>();

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            if (appSettings?.Secret != null)
            {
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            if (context == null || context.HttpContext == null)
                            {
                                return;
                            }

                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var identName = context.Principal?.Identity?.Name;
                            if (string.IsNullOrEmpty(identName))
                            {
                                context.Fail("Unauthorized");
                                return;
                            }

                            if (Guid.TryParse(identName, out var userId))
                            {
                                var user = await userService.GetByIdAsync(userId);
                                if (user == null)
                                {
                                    context.Fail("Unauthorized"); // return unauthorized if user no longer exists
                                }
                                else if (context.HttpContext.Items != null)
                                {
                                    context.HttpContext.Items["User"] = user;
                                }
                            }
                            else
                            {
                                context.Fail("Invalid user ID format");
                            }
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            }
            else
            {
                throw new InvalidOperationException("AppSettings:Secret configuration is missing");
            }

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Spestqnko.Api", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.ConfigureMigrations()
                .If(() => env.IsDevelopment(), app => 
                {
                    app.UseMigrationsEndPoint();
                    app.UseExceptionHandler("/error");
                    app.UseDeveloperExceptionPage();
                })
                .Else(app => app.UseHsts())
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spestqnko.Api V1");
                });
        }
    }
}
