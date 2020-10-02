using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeiXinOA.Models.ApplyForm;
using System.IO;
using Spire.Doc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WeiXinOA.Models.Services
{
    public class WXFileService
    {
        private readonly WXFormService _dbService;

        private static readonly object _LockVolunteerSample = new object();
        private static readonly object _LockElderSample = new object();

        //Sample位置
        private readonly string _sampleVolunteer;
        private readonly string _sampleElder;

        private readonly string _elderIdPhotoDir;
        private readonly string _volunteerIdPhotoDir;

        #region 初始化
        public WXFileService(WXFormService dbService)
        {
            _dbService = dbService;
            _sampleVolunteer = WXHostEnvHelper.VolunteerSampleDocx;
            _sampleElder = WXHostEnvHelper.ElderSampleDocx;
            _elderIdPhotoDir = WXHostEnvHelper.ElderIdPhotoDirectory;
            _volunteerIdPhotoDir = WXHostEnvHelper.VolunteerIdPhotoDirectory;
        }
        #endregion

        // Volunteer
        #region 获取 volunteer Docx 
        public async Task<ServiceResult<MemoryStream>> GetVolunteerDocxById(int id)
        {
            var result = new ServiceResult<MemoryStream>();

            //检查样品文件
            if (!File.Exists(_sampleVolunteer))
            {
                result.Error = "样本文件丢失";
                return result;
            }

            //获取数据
            var query = await _dbService.GetVolunteerAndFamilyById(id);
            if (!query.IsSuccess)
            {
                result.Error = query.Error;
                return result;
            }

            try
            {
                WriteVolunteerDocx(out MemoryStream fs, query);
                //成功返回
                result.IsSuccess = true;
                result.Value = fs;
                result.ValueName = $"{query.Volunteer.Name}-{query.Volunteer.IdNumber}.docx";
                result.MimeType = "application/msword";
                return result;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }

        }

        private void WriteVolunteerDocx(out MemoryStream mem, QueryVolunteerResult query)
        {
            lock (_LockVolunteerSample)
            {
                try
                {
                    var volunteer = query.Volunteer;
                    var family = query.Family;

                    Document doc = new Document();
                    doc.LoadFromFile(_sampleVolunteer);

                    //义工信息
                    DocReplaceText(ref doc, "Y1", volunteer.Name);
                    DocReplaceText(ref doc, "Y2", volunteer.Sex);
                    DocReplaceText(ref doc, "Y3", volunteer.Age);
                    DocReplaceText(ref doc, "Y4", volunteer.Phone);
                    DocReplaceText(ref doc, "Y5", volunteer.BirthDate);
                    DocReplaceText(ref doc, "Y6", volunteer.IdNumber);
                    DocReplaceText(ref doc, "Y7", volunteer.MinZu);
                    DocReplaceText(ref doc, "Y8", volunteer.Marital);
                    DocReplaceText(ref doc, "Y9", volunteer.PoliticalStatus);
                    DocReplaceText(ref doc, "Y10", volunteer.JiGuan);
                    DocReplaceText(ref doc, "Y11", volunteer.Education);
                    DocReplaceText(ref doc, "Y12", volunteer.ProfessinalAbility);
                    DocReplaceText(ref doc, "Y13", volunteer.GraduateInstitutions);
                    DocReplaceText(ref doc, "Y14", volunteer.HealthStatus);
                    DocReplaceText(ref doc, "Y15", volunteer.WorkUnit);
                    DocReplaceText(ref doc, "Y16", volunteer.ProfessionStatus);
                    DocReplaceText(ref doc, "Y17", volunteer.HuJiAddress);
                    DocReplaceText(ref doc, "Y18", volunteer.HomeAddress);
                    DocReplaceText(ref doc, "Y19", volunteer.WorkHistory);
                    DocReplaceText(ref doc, "Y20", volunteer.ServiceTerm);
                    DocReplaceText(ref doc, "AddTime", volunteer.AddTime);

                    //紧急联系人信息
                    DocReplaceText(ref doc, "F1", family.Name);
                    DocReplaceText(ref doc, "F2", family.Sex);
                    DocReplaceText(ref doc, "F3", family.Age);
                    DocReplaceText(ref doc, "F4", family.Relationship);
                    DocReplaceText(ref doc, "F5", family.Phone);
                    DocReplaceText(ref doc, "F6", family.HomeAddress);

                    mem = new MemoryStream();
                    doc.SaveToStream(mem, FileFormat.Docx2013);
                    doc.Close();
                    mem.Seek(0, SeekOrigin.Begin);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void DocReplaceText(ref Document document, string match, string newValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newValue))
                {
                    newValue = " ";
                }

                document.Replace(match, newValue.Trim(), false, true);
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        // Edler
        #region 获取 Elder Docx
        public async Task<ServiceResult<MemoryStream>> GetElderDocxById(int id)
        {
            var result = new ServiceResult<MemoryStream>();

            //检查样品文件           
            if (!File.Exists(_sampleElder))
            {
                result.Error = "样本文件丢失";
                return result;
            }

            //获取数据
            var query = await _dbService.GetElderAndFamilyById(id);
            if (!query.IsSuccess)
            {
                result.Error = query.Error;
                return result;
            }

            //elder 数据 写入 docx
            try
            {
                WriteElderDocx(out MemoryStream fs, query);
                result.Value = fs;
                result.IsSuccess = true;
                result.ValueName = $"{query.Elder.Name}-{query.Elder.IdNumber}.docx";
                result.MimeType = "application/msword";
                return result;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }

        private void WriteElderDocx(out MemoryStream mem, QueryElderResult query)
        {
            lock (_LockElderSample)
            {
                var elder = query.Elder;
                var familys = query.FamilyList;
                 
                try
                {
                    Document doc = new Document();
                    doc.LoadFromFile(_sampleElder);

                    //填写老人信息
                    DocReplaceText(ref doc, "L1", elder.Name);
                    DocReplaceText(ref doc, "L2", elder.Sex);
                    DocReplaceText(ref doc, "L3", elder.Age);
                    DocReplaceText(ref doc, "L4", elder.BirthDate);
                    DocReplaceText(ref doc, "L5", elder.IdNumber);
                    DocReplaceText(ref doc, "L6", elder.Marital);
                    DocReplaceText(ref doc, "L7", elder.Education);
                    DocReplaceText(ref doc, "L8", elder.Phone);
                    DocReplaceText(ref doc, "L9", elder.MinZu);
                    DocReplaceText(ref doc, "L10", elder.JiGuan);
                    DocReplaceText(ref doc, "L11", elder.HuJiAddress);
                    DocReplaceText(ref doc, "L12", elder.HomeAddress);
                    DocReplaceText(ref doc, "L13", elder.WorkUnit);
                    DocReplaceText(ref doc, "L14", elder.InCome);
                    DocReplaceText(ref doc, "L15", elder.RetirementDate);
                    DocReplaceText(ref doc, "L16", elder.HealthState);
                    DocReplaceText(ref doc, "L17", elder.MentalState);
                    DocReplaceText(ref doc, "L18", elder.ChangBeiYaoWu);
                    DocReplaceText(ref doc, "L19", elder.AiHao);
                    DocReplaceText(ref doc, "L20", elder.DietaryHabit);
                    DocReplaceText(ref doc, "L21", elder.Faith);
                    DocReplaceText(ref doc, "L22", elder.FaithTime);
                    DocReplaceText(ref doc, "L23", elder.XiuXingFaMen);
                    DocReplaceText(ref doc, "L24", elder.HomeWork);
                    DocReplaceText(ref doc, "B1", elder.Remark);
                    DocReplaceText(ref doc, "AddTime", elder.AddTime);

                    //填写亲属信息1
                    DocReplaceText(ref doc, "Q1", familys[0].Name);
                    DocReplaceText(ref doc, "Q2", familys[0].Sex);
                    DocReplaceText(ref doc, "Q3", familys[0].Relationship);
                    DocReplaceText(ref doc, "Q4", familys[0].Phone);
                    DocReplaceText(ref doc, "Q5", familys[0].Age);
                    DocReplaceText(ref doc, "Q6", familys[0].Profession);
                    DocReplaceText(ref doc, "Q7", familys[0].Education);
                    DocReplaceText(ref doc, "Q8", familys[0].IdNumber);
                    DocReplaceText(ref doc, "Q9", familys[0].HomeAddress);

                    //填写亲属信息2
                    DocReplaceText(ref doc, "2Q1", familys[1].Name);
                    DocReplaceText(ref doc, "2Q2", familys[1].Sex);
                    DocReplaceText(ref doc, "2Q3", familys[1].Relationship);
                    DocReplaceText(ref doc, "2Q4", familys[1].Phone);
                    DocReplaceText(ref doc, "2Q5", familys[1].Age);
                    DocReplaceText(ref doc, "2Q6", familys[1].Profession);
                    DocReplaceText(ref doc, "2Q7", familys[1].Education);
                    DocReplaceText(ref doc, "2Q8", familys[1].IdNumber);
                    DocReplaceText(ref doc, "2Q9", familys[1].HomeAddress);

                    mem = new MemoryStream();
                    doc.SaveToStream(mem, FileFormat.Docx2013);
                    doc.Close();
                    mem.Seek(0, SeekOrigin.Begin);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region 下载身份证图片 内存流
        /// <summary>
        /// 下载身份证图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<MemoryStream>> GetVolunteerPhotoStreamById(int id)
        {
            var result = new ServiceResult<MemoryStream>();

            var query = await _dbService.GetVolunteerById(id);
            if (!query.IsSuccess)
            {
                result.Error = query.Error;
                return result;
            }

            var path = query.Value.IdPhotoPath;

            //读取图片
            byte[] file = await File.ReadAllBytesAsync(path);

            try
            {
                //写入内存流
                MemoryStream mem = new MemoryStream(file);
                mem.Seek(0, SeekOrigin.Begin);

                result.IsSuccess = true;
                result.Value = mem;
                return result;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 获取 义工 身份证图片路径
        public async Task<ServiceResult<string>> GetVolunteerPhotoPath(int id)
        {
            var query = new ServiceResult<string>();

            var v = await _dbService.GetVolunteerById(id);

            try
            {
                if (!v.IsSuccess)
                {
                    throw new Exception(v.Error);
                }

                var file = Path.Combine(_volunteerIdPhotoDir, v.Value.IdPhotoPath);
                if (!File.Exists(file))
                {
                    throw new Exception("图片不存在");
                }

                query.Value = file;
                query.ValueName = Path.GetFileName(file);
                query.MimeType = FileMimeType.GetMimeType(Path.GetExtension(file));
                query.IsSuccess = true;
                return query;
            }
            catch (Exception ex)
            {
                query.Error = ex.Message;
                return query;
            }
        }
        #endregion

        #region 获取 老人 身份证图片路径
        public async Task<ServiceResult<string>> GetElderPhotoPath(int id)
        {
            var query = new ServiceResult<string>();

            var e = await _dbService.GetElderById(id);

            try
            {
                if (!e.IsSuccess)
                {
                    throw new Exception(e.Error);
                }

                var file = Path.Combine(_elderIdPhotoDir, e.Value.IdPhotoPath);
                if (!File.Exists(file))
                {
                    throw new Exception("图片不存在");
                }

                query.Value = file; //图片路径
                query.ValueName = Path.GetFileName(file); //图片文件名
                query.MimeType = FileMimeType.GetMimeType(Path.GetExtension(file));
                query.IsSuccess = true;
                return query;
            }
            catch (Exception ex)
            {
                query.Error = ex.Message;
                return query;
            }
        }
        #endregion
    }
}
