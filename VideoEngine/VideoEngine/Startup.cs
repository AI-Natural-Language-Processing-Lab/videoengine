using System;
using Jugnoon.Framework;
using Jugnoon.Models;
using Jugnoon.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Jugnoon.Core;
using Jugnoon.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Jugnoon.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Hosting;
using reCAPTCHA.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;

namespace VideoEngine
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
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // Global configurations 
            var _configRoot = (IConfigurationRoot)this.Configuration;
            
            // direct readable settings
            services.Configure<SiteConfiguration>(Configuration.GetSection("SiteSettings"));
            services.ConfigureWritable<Jugnoon.Settings.Database>(Configuration.GetSection("DB_Settings"), _configRoot);

            services.ConfigureWritable<Jugnoon.Settings.General>(Configuration.GetSection("General_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Premium>(Configuration.GetSection("Premium_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Smtp>(Configuration.GetSection("Smtp"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Media>(Configuration.GetSection("Media_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Features>(Configuration.GetSection("Feature_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Listing>(Configuration.GetSection("Listing_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Authentication>(Configuration.GetSection("Auth_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Registration>(Configuration.GetSection("Registration_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Aws>(Configuration.GetSection("AWS_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Social>(Configuration.GetSection("Social_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Contact>(Configuration.GetSection("Contact_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Comments>(Configuration.GetSection("Comment_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Location>(Configuration.GetSection("Location_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Settings.Rechapcha>(Configuration.GetSection("RecaptchaSettings"), _configRoot);

            // videos (usable if videos module available)
            services.ConfigureWritable<Jugnoon.Videos.Settings.General>(Configuration.GetSection("General_Video_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Videos.Settings.Aws>(Configuration.GetSection("Aws_Video_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Videos.Settings.Ffmpeg>(Configuration.GetSection("Ffmpeg_Video_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Videos.Settings.Youtube>(Configuration.GetSection("Youtube_Video_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Videos.Settings.Direct>(Configuration.GetSection("Direct_Video_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Videos.Settings.Movie>(Configuration.GetSection("Movie_General_Settings"), _configRoot);
            services.ConfigureWritable<Jugnoon.Videos.Settings.Player>(Configuration.GetSection("Player_Video_Settings"), _configRoot);
          

            if (Configuration["DB_Settings:host"] != "" 
                && Configuration["DB_Settings:database"] != "" 
                && Configuration["DB_Settings:userid"] != "" 
                && Configuration["DB_Settings:password"] != "")
            {
                
                // rechapcha
                var recaptcha = Configuration.GetSection("RecaptchaSettings");
                if (!recaptcha.Exists())
                    throw new ArgumentException("Missing RecaptchaSettings in configuration.");

                services.Configure<RecaptchaSettings>(recaptcha);
                services.AddTransient<IRecaptchaService, RecaptchaService>();

                var conn = "Server=" + Configuration["DB_Settings:host"] + "; Database=" + Configuration["DB_Settings:database"] + "; uid=" + Configuration["DB_Settings:userid"] + ";pwd=" + Configuration["DB_Settings:password"] + ";";  // Configuration.GetConnectionString("DefaultConnection");
                // setup database connectionstring.
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn), ServiceLifetime.Transient);
                
                services.AddIdentity<ApplicationUser, ApplicationRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();

                if (Configuration["Auth_Settings:enable_facebook"] == "true")
                {
                    services.AddAuthentication().AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = Configuration["Auth_Settings:fb_appId"];
                        facebookOptions.AppSecret = Configuration["Auth_Settings:fb_appSecrete"];
                    });
                }

                if (Configuration["Auth_Settings:enable_twitter"] == "true")
                {
                    services.AddAuthentication().AddTwitter(twitterOptions =>
                    {
                        twitterOptions.ConsumerKey = Configuration["Auth_Settings:tw_consumer_key"];
                        twitterOptions.ConsumerSecret = Configuration["Auth_Settings:tw_consumer_secrete"];
                    });
                }

                if (Configuration["Auth_Settings:enable_google"] == "true")
                {
                    services.AddAuthentication().AddGoogle(googleOptions =>
                    {
                        googleOptions.ClientId = Configuration["Auth_Settings:google_clientid"];
                        googleOptions.ClientSecret = Configuration["Auth_Settings:google_clientsecrete"];
                    });
                }
               
                services.AddTransient<IEmailSender, EmailSender>();

                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 6;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;

                    options.SignIn.RequireConfirmedEmail = true;
                    // User settings
                    options.User.AllowedUserNameCharacters =
                       "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;
                });

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = false;
                    options.ExpireTimeSpan = TimeSpan.FromDays(150);
                    // If the LoginPath isn't set, ASP.NET Core defaults 
                    // the path to /Account/Login.
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                    // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                    // the path to /Account/AccessDenied.
                    options.AccessDeniedPath = "/accessdenied";
                    options.SlidingExpiration = true;
                });

                // JWT Token Authorization (Mobile & Web Front App)
                /*if (Configuration["General_Settings:jwt_private_key"] != "")
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = Configuration["SiteSettings:URL"],
                            ValidIssuer = Configuration["SiteSettings:URL"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["General_Settings:jwt_private_key"]))
                        };
                    });
                }*/
            }

            // Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Cache
            services.AddMemoryCache();

            // CORS Angular Application
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }));

            services.AddRazorPages()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            if (Configuration["DB_Settings:host"] != ""
              && Configuration["DB_Settings:database"] != ""
              && Configuration["DB_Settings:userid"] != ""
              && Configuration["DB_Settings:password"] != "")
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();
                }
            }
            
            // Enable for loadbalancer
            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                return next();
            });
                      
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            var enUS = new CultureInfo("en-US");
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar-SA"),
                new CultureInfo("de-DE"),
                new CultureInfo("es-ES"),
                new CultureInfo("fr-FR"),
                new CultureInfo("it-IT"),
                new CultureInfo("pt-BR"),
                new CultureInfo("ru-RU"),
                new CultureInfo("tr-TR"),
                new CultureInfo("ja-JP"),
                new CultureInfo("zh-CHS")
            };

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUS),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (Configuration["DB_Settings:host"] != ""
             && Configuration["DB_Settings:database"] != ""
             && Configuration["DB_Settings:userid"] != ""
             && Configuration["DB_Settings:password"] != "")
            {     
                app.UseEndpoints(endpoints => RouteConfig.Use(endpoints));
            }
            else
            {
                // Setup Route (Please remove in production version)
                app.UseEndpoints(endpoints => InitRouteControllerConfig.Use(endpoints));
            }

        }
    }
}
