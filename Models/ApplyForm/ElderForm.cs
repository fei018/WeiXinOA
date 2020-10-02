using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Models.ApplyForm
{
    public class ElderForm
    {
        private string _photoDir { get; }

        public ElderForm()
        {
            _photoDir = WXHostEnvHelper.ElderIdPhotoDirectory;
        }

        public ElderDetails Elder { get; set; }

        public ElderFamily Family1 { get; set; }

        public ElderFamily Family2 { get; set; }

        public IFormFile IdPhoto { get; set; }

        #region elder 额外属性赋值
        /// <summary>
        /// Set: AddTime, Family.ElderIdNumber, IdPhotoPath
        /// </summary>
        public void SetElderOtherProp()
        {
            if (Elder == null || Family1 == null || Family2 == null)
            {
                return;
            }

            //申请表提交时间
            Elder.AddTime = DateTime.Now.ToString();

            //义工id赋值family
            Family1.ElderIdNumber = Elder.IdNumber;
            Family2.ElderIdNumber = Elder.IdNumber;

            //赋值身份证保存路径
            Elder.IdPhotoPath = GetIdPhotoFileName();
        }
        #endregion

        /// <summary>
        /// 老人身份证图片文件名
        /// </summary>
        /// <returns></returns>
        public string GetIdPhotoFileName()
        {
            if (IdPhoto.Length <= 0 || Elder == null)
            {
                return null;
            }
            var ext = Path.GetExtension(IdPhoto.FileName);
            return $"{Elder.Name}-{Elder.IdNumber}{ext}";
        }

        /// <summary>
        /// 保存身份证图片
        /// </summary>
        /// <exception cref="throw"></exception>
        public async Task SaveIdPhoto()
        {
            var savePath = Path.Combine(_photoDir, GetIdPhotoFileName());

            try
            {
                if (savePath == null)
                {
                    throw new Exception("保存路径为Null");
                }

                //如果同名文件则删除
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                //保存图片
                using var stream = File.Create(savePath);
                await IdPhoto.CopyToAsync(stream);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
