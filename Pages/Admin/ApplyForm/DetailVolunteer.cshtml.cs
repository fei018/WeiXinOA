using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.ApplyForm;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.Admin.ApplyForm
{
    public class DetailVolunteerModel : PageModel
    {
        private readonly WXFileService _fileService;
        private readonly WXFormService _formService;

        public DetailVolunteerModel(WXFileService fileService, WXFormService formService)
        {
            _fileService = fileService;
            this._formService = formService;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToPage("/Error", new { error = "volunteer id null" });
            }

            var query = await _formService.GetVolunteerById(id.Value);
            if (!query.IsSuccess)
            {
                return RedirectToPage("/Error", new { error = $"volunteer id={id.Value} 不存在" });
            }
            else
            {
                Volunteer = query.Value;
                return Page();
            }
        }

        [BindProperty]
        public VolunteerDetails Volunteer { get; set; }

        #region 下载 Docx
        public async Task<IActionResult> OnPostDownloadDocx()
        {
            var query = await _fileService.GetVolunteerDocxById(Volunteer.Id);
            if (query.IsSuccess)
            {
                return File(query.Value, query.MimeType, query.ValueName);
            }
            else
            {               
                ModelState.AddModelError(string.Empty, query.Error);
                return await OnGet(Volunteer.Id);
            }
        }
        #endregion

        #region 下载身份证图片
        public async Task<IActionResult> OnPostDownloadIdPhoto()
        {
            var query = await _fileService.GetVolunteerPhotoPath(Volunteer.Id);
            if (query.IsSuccess)
            {
                return PhysicalFile(query.Value, query.MimeType, query.ValueName);
            }
            else
            {
                ModelState.AddModelError(string.Empty, query.Error);
                return await OnGet(Volunteer.Id);
            }
        }
        #endregion

        #region 删除 volunteer and family
        public async Task<IActionResult> OnPostDeleteVolunteer()
        {
            await _formService.DeleteVolunteerById(Volunteer.Id);

            return RedirectToPage("Volunteer");
        }
        #endregion
    }
}
