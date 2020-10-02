using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WeiXinOA.Models.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Text.Unicode;
using WeiXinOA.Models.Services;
using WeiXinOA.Models.ApplyForm;
using WeiXinOA.Models.Account;

namespace WeiXinOA
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
            //配置授权策略
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("EditAccount", policy => policy.RequireRole(LoginUserRole.Administrator));
                opt.AddPolicy("DetailsFormData", policy => policy.RequireRole(LoginUserRole.Administrator, LoginUserRole.PowerUser));
            });

            services.AddRazorPages()
                    .AddRazorPagesOptions(opt =>
                    {   
                        // 授权访问 admin
                        opt.Conventions.AuthorizeFolder("/Admin");
                    
                        // 授权编辑 登录账户
                        opt.Conventions.AuthorizeFolder("/Admin/Account", "EditAccount");

                        // 授权下载 老人义工数据
                        opt.Conventions.AuthorizePage("/Admin/ApplyForm/DetailElder", "DetailsFormData");
                        opt.Conventions.AuthorizePage("/Admin/ApplyForm/DetailVolunteer", "DetailsFormData");
                    });

            //配置验证cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Login/Index");
                    options.AccessDeniedPath = new PathString("/AccessDenied");
                    options.LogoutPath = new PathString("/Login/Index");
                });

            //配置数据库
            services.AddDbContextPool<WeiXinOADbContext>(opts =>
            {
                //opts.UseSqlServer(Configuration.GetConnectionString("WeiXinOADb"), b => b.MigrationsAssembly("WeiXinOA"));
                opts.UseSqlServer(WXHostEnvHelper.ConnectionString, b => b.MigrationsAssembly("WeiXinOA"));
            });

            //输出html时 不编码 拉丁, 中文
            services.AddSingleton(System.Text.Encodings.Web.HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));

            //注入 HttpContext 访问器
            services.AddHttpContextAccessor();

            //注入WeiXinOA服务
            services.AddScoped<WXFormService>();
            services.AddScoped<WXFileService>();
            services.AddScoped<WXLoginService>();
            services.AddScoped<WXMailService>();
            services.AddScoped<VolunteerForm>();
            services.AddScoped<ElderForm>();
            services.AddScoped<WXHostEnvHelper>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            //客户端显示异常页面
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions
            {
                //设置不限制content-type
                //ServeUnknownFileTypes = true
            });

            app.UseRouting();

            //添加验证
            app.UseAuthentication();

            //验证授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }


    }
}
