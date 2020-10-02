using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.ApplyForm;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.ApplyForm
{
    public class ElderModel : PageModel
    {
        private readonly WXFormService _dbService;
        private readonly WXMailService _mailService;
        private readonly ElderForm _elderForm;

        public ElderModel(WXFormService dbService, WXMailService mailService, ElderForm elderForm)
        {
            this._dbService = dbService;
            this._mailService = mailService;
            this._elderForm = elderForm;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public ElderDetails ElderDetails { get; set; }

        [BindProperty]
        public ElderFamily ElderFamily1 { get; set; }

        [BindProperty]
        public ElderFamily ElderFamily2 { get; set; }

        [BindProperty]
        public IFormFile IdPhoto { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError(string.Empty, "有未填项 或 填错");
                return Page();
            }

            //验证图片大小
            if (IdPhoto.Length <= 0)
            {
                ModelState.AddModelError(string.Empty, "身份证图片无效,请重新选择");
                return Page();
            }

            //_elderForm赋值
            _elderForm.Elder = ElderDetails;
            _elderForm.Family1 = ElderFamily1;
            _elderForm.Family2 = ElderFamily2;
            _elderForm.IdPhoto = IdPhoto;

            //写入数据库
            var query = await _dbService.SaveElderToDb(_elderForm);

            //写入是否成功
            if (query.IsSuccess)
            {
                //发送邮件通知
                await _mailService.SendMail("新增老人申请表", $"姓名: {ElderDetails.Name}");
                return RedirectToPage("FormResult", new { Result = "提交成功，谢谢!" });
            }
            else
            {
                return RedirectToPage("FormResult", new { Result = query.Error });
            }
        }
    }
}
