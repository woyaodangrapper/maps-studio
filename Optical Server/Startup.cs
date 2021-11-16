using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Threading.Tasks;
using static Taoist.Archives.project.Web;

namespace OpticalServer
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

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new OpenApiInfo { Title = "中间件服务可视化文档", Version = "v3" });
                //Determine base path for the application.  
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(basePath, "OpticalServer.xml");
                c.IncludeXmlComments(xmlPath);
            });


            {
                #region 跨域
                // 添加跨域策略
                services.AddCors(options => {
                    options.AddPolicy(AllowSpecificOrigin, builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
                });
                services.AddControllers();


                #endregion
                //配置返回Json
                services.AddControllersWithViews().AddNewtonsoftJson();
            }
            {
                // STEP1: 設定用哪種方式驗證 HTTP Request 是否合法
                services
                    // 檢查 HTTP Header 的 Authorization 是否有 JWT Bearer Token
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    // 設定 JWT Bearer Token 的檢查選項
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["Jwt:Issuer"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw=="))
                        };
                    });
            }

        }
        public class CorsMiddleware
        {
            private readonly RequestDelegate _next;
            public CorsMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
                await _next(context);
            }
        }


        private readonly string AllowSpecificOrigin = "AllowSpecificOrigin";

        UseDirectoryBrowser useDirectoryBrowser = new UseDirectoryBrowser();
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var c = useDirectoryBrowser.Configure(@"D:\数据\模型数据");
#if DEBUG
            System.Diagnostics.Process.Start("explorer", c.url);
#endif


            app.UseMiddleware<CorsMiddleware>();

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {//ISC_API/
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // STEP2: 使用驗證權限的 Middleware
            app.UseAuthentication();

            app.UseHttpsRedirection();
            //设置远程
            app.UseRouting();
            //CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行。
            app.UseCors(AllowSpecificOrigin);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
