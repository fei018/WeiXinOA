using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.Account;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly WXLoginService _loginService;

        public IndexModel(WXLoginService loginService)
        {
            this._loginService = loginService;
        }

        public void OnGet()
        {
            
        }

        [BindProperty]
        public LoginUser LoginUser { get; set; }

        #region µ«»Î
        public async Task<IActionResult> OnPostLogin()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var ok = await _loginService.Login(LoginUser.LoginName, LoginUser.Password);
            if (ok)
            {
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "’À∫≈ªÚ√‹¬Î¥ÌŒÛ");
                return Page();
            }
        }
        #endregion

        #region µ«≥ˆ
        public async Task<IActionResult> OnGetLogout()
        {
            await _loginService.Logout();
            return RedirectToPage();
        }
        #endregion
    }
}
