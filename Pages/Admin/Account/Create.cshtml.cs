using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.Account;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.Admin.Account
{
    //[Authorize(Roles = LoginUserRole.Administrator)]
    public class CreateModel : PageModel
    {
        private readonly WXLoginService _loginService;

        public CreateModel(WXLoginService loginService)
        {
            _loginService = loginService;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public LoginUser NewUser { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var query = await _loginService.CreateLoginUser(NewUser);
            if (!query.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, query.Error);
                return Page();
            }

            return RedirectToPage("/Admin/Account/Index");
        }
    }
}
