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
    public class VolunteerModel : PageModel
    {
        private readonly WXFormService _dbService;
        private readonly WXMailService _mailService;
        private readonly VolunteerForm _volunteerForm;

        public VolunteerModel(WXFormService dbService, WXMailService mailService, VolunteerForm volunteerForm)
        {
            _dbService = dbService;
            _mailService = mailService;
            _volunteerForm = volunteerForm;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public VolunteerDetails Volunteer { get; set; }

        [BindProperty]
        public VolunteerFamily Family { get; set; }

        [BindProperty]
        public IFormFile IdPhoto { get; set; }

        public async Task<IActionResult> OnPost()
        { 
            //验证表单
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError(string.Empty, "有未填项 或 填错");
                return Page();
            }

            //验证图片无效
            if (IdPhoto.Length <= 0)
            {
                ModelState.AddModelError(string.Empty, "身份证图片无效,请重新选择");
                return Page();
            }

            //_volunteerForm 赋值
            _volunteerForm.Volunteer = Volunteer;
            _volunteerForm.Family = Family;
            _volunteerForm.IdPhoto = IdPhoto;

            //写入数据库
            var query = await _dbService.SaveVolunteerToDb(_volunteerForm);

            //写入数据是否成功
            if (query.IsSuccess)
            {
                //发送邮件
                await _mailService.SendMail("新增义工申请表", $"姓名: {Volunteer.Name}");
                return RedirectToPage("FormResult", new { Result = "提交成功，谢谢!" });
            }
            else
            {
                return RedirectToPage("FormResult", new { Result = query.Error });
            }
        }
      
    }
}
