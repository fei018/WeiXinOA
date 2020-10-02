using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

namespace WeiXinOA.Models.Services
{
    public class WXHostEnvHelper
    {
        public WXHostEnvHelper(IHttpContextAccessor contextAccessor)
        {
            //ContentRootPath = host.ContentRootPath;
            //WebRootPath = host.WebRootPath;
            //HostConfiguration = configuration;

            HttpContext = contextAccessor.HttpContext;           
        }

        //public IConfiguration HostConfiguration { get;}
        //public string ContentRootPath { get; }
        //public string WebRootPath { get; }
        public HttpContext HttpContext { get;}

        

        /// <summary>
        /// 获取 WeiXinOASet 目录
        /// </summary>
        //public string WeiXinOASet => Path.Combine(Directory.GetParent(ContentRootPath).FullName, "WeiXinOASet");
        public static string WeiXinOASet => Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "WeiXinOASet");

        /// <summary>
        /// 获取 WXOAConfig 配置文件
        /// </summary>
        public static string WXOAConfig => Path.Combine(WeiXinOASet, "WXOAConfig.json");

        /// <summary>
        /// 获取 Elder IdPhoto 目录
        /// </summary>
        public static string ElderIdPhotoDirectory => Path.Combine(WeiXinOASet, "IdPhoto\\Elder");

        /// <summary>
        /// 获取 Volunteer IdPhoto 目录
        /// </summary>
        public static string VolunteerIdPhotoDirectory => Path.Combine(WeiXinOASet, "IdPhoto\\Volunteer");

        /// <summary>
        /// 义工样本Docx
        /// </summary>
        public static string VolunteerSampleDocx => Path.Combine(WeiXinOASet, @"WeiXinSample\义工申请表.docx");

        /// <summary>
        /// 老人样本Docx
        /// </summary>
        public static string ElderSampleDocx => Path.Combine(WeiXinOASet, @"WeiXinSample\老人申请表.docx");

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public static string ConnectionString => GetWXOAConfigValue("ConnectionStrings", "WeiXinOADb");

        /// <summary>
        /// 获取 配置文件节点的值
        /// </summary>
        public static string GetWXOAConfigValue(string nodeName)
        {
            using var file = File.OpenRead(WXOAConfig);
            using var json = JsonDocument.Parse(file);
            var element = json.RootElement;
            var value = element.GetProperty(nodeName).GetString();
            return value;
        }

        /// <summary>
        /// 获取 配置文件节点的值
        /// </summary>
        public static string GetWXOAConfigValue(string sectionName, string nodeName)
        {           
            using var file = File.OpenRead(WXOAConfig);
            using var json = JsonDocument.Parse(file);
            var element = json.RootElement;
            var value = element.GetProperty(sectionName).GetProperty(nodeName).GetString();
            return value;
        }
    }
}
