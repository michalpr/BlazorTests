namespace BlazorCookieAuth
{
    using BlazorCookieAuth.Authentication;
    using BlazorCookieAuth.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Radzen;
    using System.Security.Claims;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            services.AddSingleton<DbDataService>();
            services.AddScoped<IDbDataService, DbDataService>();

            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            //services.AddAuthorization(options => {
            //    options.AddPolicy("MustBeVIA",  a => 
            //        a.RequireAuthenticatedUser().RequireClaim("Domain", "via.dk"));

            //    options.AddPolicy("SecurityLevel4",  a => 
            //        a.RequireAuthenticatedUser().RequireClaim("Level", "4","5"));

            //    options.AddPolicy("MustBeTeacher",  a => 
            //        a.RequireAuthenticatedUser().RequireClaim("Role", "Teacher"));

            //    options.AddPolicy("SecurityLevel2", policy =>
            //        policy.RequireAuthenticatedUser().RequireAssertion(context => {
            //            Claim levelClaim = context.User.FindFirst(claim => claim.Type.Equals("Level"));
            //            if (levelClaim == null) return false;
            //            return int.Parse(levelClaim.Value) >= 2;
            //        }));
            //});
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}