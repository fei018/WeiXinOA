using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WeiXinOA.Models.Account;
using WeiXinOA.Models.Database;

namespace WeiXinOA.Models.Services
{
    public class WXLoginService
    {
        private readonly WeiXinOADbContext _dbContext;
        private readonly HttpContext _httpContext;

        public WXLoginService(WeiXinOADbContext dbContext, WXHostEnvHelper hostEnvHelper)
        {
            this._dbContext = dbContext;
            this._httpContext = hostEnvHelper.HttpContext;
        }


        #region 登入
        public async Task<bool> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = GetUserFromDbContext(userName, password);
            if (user == null) return false;

            await SignIn(user);
            return true;
        }

        private async Task SignIn(LoginUser user)
        {
            var claimIndentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIndentity.AddClaim(new Claim(ClaimTypes.Name, user.LoginName));
            claimIndentity.AddClaim(new Claim(ClaimTypes.Role, user.RoleName));
            var principal = new ClaimsPrincipal(claimIndentity);

            //验证参数内容
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                //是否永久保存cookie
                IsPersistent = false
            };

            // 在上面注册AddAuthentication时，指定了默认的Scheme，在这里便可以不再指定Scheme。
            await _httpContext.SignInAsync(principal, authProperties);
        }

        private LoginUser GetUserFromDbContext(string userName, string password)
        {
            return _dbContext.Tbl_LoginUser.FirstOrDefault(u => u.LoginName == userName && u.Password == password);
        }
        #endregion

        #region 登出
        public async Task Logout()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                await _httpContext.SignOutAsync();
            }
        }
        #endregion

        #region Get LoginUser List
        public async Task<ServiceResult<List<LoginUser>>> GetLoginUserList()
        {
            var service = new ServiceResult<List<LoginUser>>();

            try
            {
                var query = await _dbContext.Tbl_LoginUser.OrderByDescending(u => u.Id).ToListAsync();
                service.SetValue(query);
                return service;
            }
            catch (System.Exception ex)
            {
                service.Error = ex.Message;
                return service;
            }
        }
        #endregion

        #region 新建 LoginUser
        public async Task<ServiceResult> CreateLoginUser(LoginUser newUser)
        {
            var result = new ServiceResult();

            var isHas = await _dbContext.Tbl_LoginUser.AnyAsync(u => u.LoginName == newUser.LoginName);
            if (isHas)
            {
                result.Error = "用户名已存在";
                return result;
            }

            await _dbContext.Tbl_LoginUser.AddAsync(newUser);
            try
            {
                await _dbContext.SaveChangesAsync();
                result.IsSuccess = true;
                return result;
            }
            catch (System.Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 更改 LoginUser RoleName
        public async Task<ServiceResult> EditLoginUserRoleName(int id, string role)
        {
            var result = new ServiceResult();

            var user = await _dbContext.Tbl_LoginUser.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                result.Error = $"id={id} 用户名不存在";
                return result;              
            }

            user.RoleName = role;
            _dbContext.Tbl_LoginUser.Update(user);
            try
            {
                await _dbContext.SaveChangesAsync();
                result.IsSuccess = true;
                return result;
            }
            catch (System.Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region 更改 LoginUser Password
        public async Task<ServiceResult> EditLoginUserPassword(int id, string password)
        {
            var result = new ServiceResult();

            var user = await _dbContext.Tbl_LoginUser.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                result.Error = $"id={id} 用户名不存在";
                return result;
            }

            user.Password = password;
            _dbContext.Tbl_LoginUser.Update(user);
            try
            {
                await _dbContext.SaveChangesAsync();
                result.IsSuccess = true;
                return result;
            }
            catch (System.Exception ex)
            {
                result.Error = ex.Message;
                return result;
            }
        }
        #endregion

        #region Get LoginUser
        public async Task<ServiceResult<LoginUser>> GetLoginUserById(int id)
        {
            var service = new ServiceResult<LoginUser>();
            var query = await _dbContext.Tbl_LoginUser.FindAsync(id);
            if (query == null)
            {
                service.Error = $"id={id} 用户名不存在";
                return service;
            }

            service.SetValue(query);
            return service;
        }
        #endregion

        #region Delete LoginUser
        public async Task<ServiceResult> DeleteLoginUserById(int id)
        {
            var service = new ServiceResult();
            var user = await _dbContext.Tbl_LoginUser.FindAsync(id);
            _dbContext.Tbl_LoginUser.Remove(user);
            try
            {
                await _dbContext.SaveChangesAsync();
                service.IsSuccess = true;
                return service;
            }
            catch (System.Exception ex)
            {
                service.Error = ex.Message;
                return service;
            }
        }
        #endregion
    }
}
