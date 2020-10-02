using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiXinOA.Models.ApplyForm;

namespace WeiXinOA.Models.Services
{
    public class QueryVolunteerResult:ServiceResult
    {
        public List<VolunteerDetails> VolunteerList { get; set; }

        public VolunteerDetails Volunteer { get; set; }

        public VolunteerFamily Family { get; set; }

        /// <summary>
        /// volunteer 查询总数
        /// </summary>
        public string TotalCount { get; set; }

        /// <summary>
        /// 返回 layui-table Json数据
        /// </summary>
        /// <returns></returns>
        public object GetLayuiTableJsonData()
        {
            return new { code = 0, msg = "ok", count = this.TotalCount, data = this.VolunteerList };
        }

        /// <summary>
        /// 返回 layui-table Json数据, error 错误信息
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public object GetLayuiTableJsonData(string error)
        {
            return new { code = 400, msg = error, count = string.Empty, data = string.Empty };
        }
    }
}
