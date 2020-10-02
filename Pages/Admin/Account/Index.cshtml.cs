using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.Account;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.Admin.Account
{
    public class IndexModel : PageModel
    {
        private readonly WXLoginService _loginService;

        public IndexModel(WXLoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<IActionResult> OnGet()
        {
            var query = await _loginService.GetLoginUserList();
            if (!query.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, query.Error);
                return Page();
            }

            UserList = query.Value;
            return Page();
        }

        public List<LoginUser> UserList { get; set; }
    }
}
