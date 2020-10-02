using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiXinOA.Models.ApplyForm;

namespace WeiXinOA.Models.Services
{
    public class QueryElderResult:ServiceResult
    {
        public List<ElderDetails> ElderList { get; set; }

        public ElderDetails Elder { get; set; }

        public List<ElderFamily> FamilyList { get; set; }

        /// <summary>
        /// Elder 查询总数
        /// </summary>
        public string TotalCount { get; set; }

        public object GetLayuiTableJsonData()
        {
            return new { code = 0, msg = "ok", count = this.TotalCount, data = this.ElderList };
        }

        public object GetLayuiTableJsonData(string error)
        {
            return new { code = 400, msg = error, count = string.Empty, data = string.Empty };
        }
    }
}
