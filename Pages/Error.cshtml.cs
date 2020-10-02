using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WeiXinOA.Pages
{
    public class ErrorModel : PageModel
    {
        public void OnGet(string error)
        {
            //ViewData["Error"] = error;
            Error = error;
        }

        public string Error { get; set; }
    }
}
