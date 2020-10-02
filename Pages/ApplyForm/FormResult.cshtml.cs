using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeiXinOA.Pages.ApplyForm
{
    public class FormResultModel : PageModel
    {
        public void OnGet(string Result)
        {
            ViewData["Result"] = Result;
        }
    }
}
