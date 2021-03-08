using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WangZhen.Techniques.Auth.Api.Extensions;
using WangZhen.Techniques.Auth.Api.Helpers;
using WangZhen.Techniques.Auth.Api.Services;

namespace WangZhen.Techniques.Auth.Api
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
            services.AddCors();
            services.Configure<JwtSetting>(Configuration.GetSection("JwtSetting"));
            services.AddScoped<IUserService, UserService>();


            services.AddControllers();

            var jwtSettingSection = Configuration.GetSection("JwtSetting");
            services.Configure<JwtSetting>(jwtSettingSection);

            var jwtSetting = jwtSettingSection.Get<JwtSetting>();
            var key = Encoding.ASCII.GetBytes(jwtSetting.Secret);


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
            });

            services.AddConsul(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth.Api v1"));
            }

            //app.UseHttpsRedirection();



            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

            // app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/healthcheck",async(ctx)=> { 
                    ctx.Response.ContentType = "text/plain";
                    await ctx.Response.WriteAsync("ok");
                });
                endpoints.MapControllers();
            });

            app.UseConsul();
        }
    }
}
