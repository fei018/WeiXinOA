using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeiXinOA.Models.Services
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }

        private string _error;

        /// <summary>
        /// 自动设置 IsSucess = false;
        /// </summary>
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                IsSuccess = false;
                _error = "错误: " + value;
            }
        }
    }

    public class ServiceResult<T> : ServiceResult where T:class
    {
        public T Value { get; set; }

        public string ValueName { get; set; }

        public string MimeType { get; set; }

        #region SetValue
        /// <summary>
        /// 自动设置 IsSucess = true;
        /// </summary>
        public void SetValue(T value)
        {
            this.IsSuccess = true;
            this.Value = value;
        }

        /// <summary>
        /// 自动设置 IsSucess = true;
        /// </summary>
        public void SetValue(T value, string valueName)
        {
            this.IsSuccess = true;
            this.Value = value;
            this.ValueName = valueName;
        }

        /// <summary>
        /// 自动设置 IsSucess = true;
        /// </summary>
        public void SetValue(T value, string valueName, string mimeType)
        {
            this.IsSuccess = true;
            this.Value = value;
            this.ValueName = valueName;
            this.MimeType = mimeType;
        }
        #endregion
    }

}
