using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Models.ApplyForm
{
    public class VolunteerForm
    {
        private string _photoDir { get; }

        public VolunteerForm()
        {
            this._photoDir = WXHostEnvHelper.VolunteerIdPhotoDirectory;
        }

        public VolunteerDetails Volunteer { get; set; }

        public VolunteerFamily Family { get; set; }

        public IFormFile IdPhoto { get; set; }


        #region Volunteer 属性 额外赋值
        /// <summary>
        /// Set: AddTime, VolunteerIdNumber, IdPhotoPath
        /// </summary>
        public void SetVolunteerOtherProp()
        {
            if (Volunteer == null || Family == null)
            {
                return;
            }

            //申请表提交时间
            Volunteer.AddTime = DateTime.Now.ToString();

            //义工id赋值family
            Family.VolunteerIdNumber = Volunteer.IdNumber;

            //赋值身份证保存路径
            Volunteer.IdPhotoPath = GetIdPhotoFileName();

        }
        #endregion

        /// <summary>
        /// 返回身份证图片文件名
        /// </summary>
        /// <returns></returns>
        public string GetIdPhotoFileName()
        {
            if (IdPhoto.Length <= 0 || Volunteer == null)
            {
                return null;
            }
            var ext = Path.GetExtension(IdPhoto.FileName);
            return $"{Volunteer.Name}-{Volunteer.IdNumber}{ext}";
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
