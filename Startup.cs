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
            //������Ȩ����
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("EditAccount", policy => policy.RequireRole(LoginUserRole.Administrator));
                opt.AddPolicy("DetailsFormData", policy => policy.RequireRole(LoginUserRole.Administrator, LoginUserRole.PowerUser));
            });

            services.AddRazorPages()
                    .AddRazorPagesOptions(opt =>
                    {   
                        // ��Ȩ���� admin
                        opt.Conventions.AuthorizeFolder("/Admin");
                    
                        // ��Ȩ�༭ ��¼�˻�
                        opt.Conventions.AuthorizeFolder("/Admin/Account", "EditAccount");

                        // ��Ȩ���� �����幤����
                        opt.Conventions.AuthorizePage("/Admin/ApplyForm/DetailElder", "DetailsFormData");
                        opt.Conventions.AuthorizePage("/Admin/ApplyForm/DetailVolunteer", "DetailsFormData");
                    });

            //������֤cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Login/Index");
                    options.AccessDeniedPath = new PathString("/AccessDenied");
                    options.LogoutPath = new PathString("/Login/Index");
                });

            //�������ݿ�
            services.AddDbContextPool<WeiXinOADbContext>(opts =>
            {
                //opts.UseSqlServer(Configuration.GetConnectionString("WeiXinOADb"), b => b.MigrationsAssembly("WeiXinOA"));
                opts.UseSqlServer(WXHostEnvHelper.ConnectionString, b => b.MigrationsAssembly("WeiXinOA"));
            });

            //���htmlʱ ������ ����, ����
            services.AddSingleton(System.Text.Encodings.Web.HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));

            //ע�� HttpContext ������
            services.AddHttpContextAccessor();

            //ע��WeiXinOA����
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

            //�ͻ�����ʾ�쳣ҳ��
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles(new StaticFileOptions
            {
                //���ò�����content-type
                //ServeUnknownFileTypes = true
            });

            app.UseRouting();

            //�����֤
            app.UseAuthentication();

            //��֤��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }


    }
}
