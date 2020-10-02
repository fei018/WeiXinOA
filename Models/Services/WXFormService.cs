using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeiXinOA.Models.ApplyForm;
using WeiXinOA.Models.Database;

namespace WeiXinOA.Models.Services
{
    public class WXFormService
    {
        private readonly WeiXinOADbContext _dbContext;

        public WXFormService(WeiXinOADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region TestDb
        //public void SetTestDb()
        //{          
        //    try
        //    {
        //        if (!_dbContext.Tbl_Volunteers.Any())
        //        {
        //            var vlist = new List<VolunteerDetails>();
        //            for (int i = 1; i < 100; i++)
        //            {
        //                var v = new VolunteerDetails
        //                {
        //                    Name = "小明" + i.ToString(),
        //                    AddTime = DateTime.Now.ToString(),
        //                    Age = "30",
        //                    BirthDate = DateTime.Now.ToShortDateString(),
        //                    Education = "大学",
        //                    GraduateInstitutions = "清华",
        //                    HealthStatus = "良好",
        //                    HomeAddress = "浙江温州文成县666号",
        //                    HuJiAddress = "北京中南海666号",
        //                    IdNumber = "666666666666666666",
        //                    JiGuan = "福建福州",
        //                    Marital = "未婚",
        //                    MinZu = "汉",
        //                    Phone = "12345678912",
        //                    PoliticalStatus = "群众",
        //                    ProfessinalAbility = "开太空飞船",
        //                    ProfessionStatus = "退休",
        //                    ServiceTerm = "长期",
        //                    Sex = "男",
        //                    WorkHistory = "没啥好说的",
        //                    WorkUnit = "无"
        //                };
        //                vlist.Add(v);
        //                _dbContext.Tbl_Volunteers.Add(v);
        //            }
        //            _dbContext.SaveChanges();
        //        }             
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        #endregion

        #region 保存 Volunteer
        public async Task<ServiceResult> SaveVolunteerToDb(VolunteerForm form)
        {
            var result = new ServiceResult();
            var volunteer = form.Volunteer;
            var family = form.Family;

            if (volunteer == null || family == null)
            {
                result.Error = "volunteer or family Null";
                return result;
            }

            #region 检查数据库是否有相同 身份证号码
            var checkIdcard = await _dbContext.Tbl_Volunteer.AnyAsync(v => v.IdNumber == volunteer.IdNumber);
            if (checkIdcard)
            {
                result.Error = "此身份证号已存在";
                return result;
            }
            #endregion           

            //义工数据额外赋值
            form.SetVolunteerOtherProp();

            //写入数据库缓存
            await _dbContext.Tbl_Volunteer.AddAsync(volunteer);
            await _dbContext.Tbl_VolunteerFamily.AddAsync(family);
            try
            {
                //写入数据库
                var i = await _dbContext.SaveChangesAsync();

                if (i > 0)
                {
                    //保存图片
                    await form.SaveIdPhoto();
                    result.IsSuccess = true;
                    return result;
                }
                else
                {
                    result.Error = "未知错误";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 保存 Elder
        public async Task<ServiceResult> SaveElderToDb(ElderForm form)
        {
            var result = new ServiceResult();
            var elder = form.Elder;
            var family1 = form.Family1;
            var family2 = form.Family2;

            if (elder == null || family1 == null || family2 == null)
            {
                result.Error = "elder or family Null";
                return result;
            }

            //检查数据库是否有相同 身份证号码
            var checkIdcard = await _dbContext.Tbl_Elder.AnyAsync(e => e.IdNumber == elder.IdNumber);
            if (checkIdcard)
            {
                result.Error = "老人身份证号已存在";
                return result;
            }

            //老人数据额外赋值
            form.SetElderOtherProp();

            //写入数据库缓存
            await _dbContext.Tbl_Elder.AddAsync(elder);
            await _dbContext.Tbl_ElderFamily.AddAsync(family1);
            await _dbContext.Tbl_ElderFamily.AddAsync(family2);

            try
            {
                //写入数据库
                var i = await _dbContext.SaveChangesAsync();
                if (i > 0)
                {
                    //保存图片
                    await form.SaveIdPhoto();
                    result.IsSuccess = true;
                    return result;
                }
                else
                {
                    result.Error = "未知错误";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 分页查询 All Volunteer
        public async Task<QueryVolunteerResult> GetAllVolunteerPaged(int pageIndex, int pageSize)
        {
            var result = new QueryVolunteerResult();

            try
            {
                var volunteer = (from v in _dbContext.Tbl_Volunteer orderby v.Id descending select v).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var query = await volunteer.ToListAsync();
                if (query == null && !query.Any())
                {
                    result.IsSuccess = false;
                    result.Error = "数据为空";
                    return result;
                }

                result.IsSuccess = true;
                result.VolunteerList = query;
                result.TotalCount = _dbContext.Tbl_Volunteer.Count().ToString();
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 查询 Volunteer list by Name
        public async Task<QueryVolunteerResult> GetVolunteerByName(string name)
        {
            var result = new QueryVolunteerResult();

            try
            {
                var volunteer = from v in _dbContext.Tbl_Volunteer where v.Name == name select v;
                var query = await volunteer.ToListAsync();
                if (query == null && !query.Any())
                {
                    result.IsSuccess = false;
                    result.Error = "数据为空";
                    return result;
                }

                result.IsSuccess = true;
                result.VolunteerList = query;
                result.TotalCount = query.Count.ToString();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                result.IsSuccess = false;
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 分页查询 All Elder
        public async Task<QueryElderResult> GetAllElderPaged(int pageIndex, int pageSize)
        {
            var result = new QueryElderResult();

            try
            {
                var elder = (from v in _dbContext.Tbl_Elder orderby v.Id descending select v).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var query = await elder.ToListAsync();
                if (query == null && !query.Any())
                {
                    result.IsSuccess = false;
                    result.Error = "数据为空";
                    return result;
                }

                result.IsSuccess = true;
                result.ElderList = query;
                result.TotalCount = _dbContext.Tbl_Elder.Count().ToString();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                result.IsSuccess = false;
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 查询 Elder list By Name
        public async Task<QueryElderResult> GetElderByName(string name)
        {
            var result = new QueryElderResult();

            try
            {
                var elder = from v in _dbContext.Tbl_Elder where v.Name == name select v;
                var query = await elder.ToListAsync();
                if (query == null && !query.Any())
                {
                    result.IsSuccess = false;
                    result.Error = "数据为空";
                    return result;
                }

                result.IsSuccess = true;
                result.ElderList = query;
                result.TotalCount = query.Count.ToString();
                return result;
            }
            catch (ArgumentNullException ex)
            {
                result.IsSuccess = false;
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 查询 volunteer & family by id
        public async Task<QueryVolunteerResult> GetVolunteerAndFamilyById(int id)
        {
            var query = new QueryVolunteerResult();

            var v = await _dbContext.Tbl_Volunteer.FindAsync(id);
            if (v == null)
            {
                query.IsSuccess = false;
                query.Error = "id数据查询不到";
                return query;
            }
            else
            {
                var f = await _dbContext.Tbl_VolunteerFamily.FirstOrDefaultAsync(q => q.VolunteerIdNumber == v.IdNumber);
                query.Volunteer = v;
                query.Family = f;
                query.IsSuccess = true;
                return query;
            }
        }
        #endregion

        #region 查询 Elder & family by id
        public async Task<QueryElderResult> GetElderAndFamilyById(int id)
        {
            var query = new QueryElderResult();

            var e = await _dbContext.Tbl_Elder.FindAsync(id);
            if (e == null)
            {
                query.IsSuccess = false;
                query.Error = "id数据查询不到";
                return query;
            }
            else
            {
                var fs = await _dbContext.Tbl_ElderFamily.Where(q => q.ElderIdNumber == e.IdNumber).ToListAsync();
                query.Elder = e;
                query.FamilyList = fs;
                query.IsSuccess = true;
                return query;
            }
        }
        #endregion

        #region 返回 volunteer by id
        public async Task<ServiceResult<VolunteerDetails>> GetVolunteerById(int id)
        {
            var query = new ServiceResult<VolunteerDetails>();

            var v = await _dbContext.Tbl_Volunteer.FindAsync(id);
            if (v == null)
            {
                query.IsSuccess = false;
                query.Error = "查无此Id";
                return query;
            }

            query.IsSuccess = true;
            query.Value = v;
            return query;
        }
        #endregion

        #region 返回 elder by id
        public async Task<ServiceResult<ElderDetails>> GetElderById(int id)
        {
            var query = new ServiceResult<ElderDetails>();

            var e = await _dbContext.Tbl_Elder.FindAsync(id);
            if (e == null)
            {
                query.IsSuccess = false;
                query.Error = "查无此Id";
                return query;
            }

            query.IsSuccess = true;
            query.Value = e;
            return query;
        }
        #endregion

        #region 删除 Elder
        public async Task DeleteElderById(int id)
        {
            var e = await _dbContext.Tbl_Elder.FindAsync(id);
            _dbContext.Tbl_Elder.Remove(e);

            var fs = await _dbContext.Tbl_ElderFamily.Where(f => f.ElderIdNumber == id.ToString()).ToListAsync();
            _dbContext.Tbl_ElderFamily.RemoveRange(fs);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion

        #region 删除 Volunteer
        public async Task DeleteVolunteerById(int id)
        {
            var v = await _dbContext.Tbl_Volunteer.FindAsync(id);
            _dbContext.Tbl_Volunteer.Remove(v);

            var fs = await _dbContext.Tbl_VolunteerFamily.Where(f => f.VolunteerIdNumber == id.ToString()).ToListAsync();
            _dbContext.Tbl_VolunteerFamily.RemoveRange(fs);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }
        }
        #endregion
    }
}
