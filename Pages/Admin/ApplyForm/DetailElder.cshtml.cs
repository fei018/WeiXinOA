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
    public class DetailElderModel : PageModel
    {
        private readonly WXFormService _formService;
        private readonly WXFileService _fileService;

        public DetailElderModel(WXFormService formService, WXFileService fileService)
        {
            _formService = formService;
            _fileService = fileService;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToPage("/Error", new { error = "Elder id null" });
            }

            var query = await _formService.GetElderById(id.Value);
            if (!query.IsSuccess)
            {
                return RedirectToPage("/Error", new { error = $"Elder id={id.Value} ������" });
            }
            else
            {
                Elder = query.Value;
                return Page();
            }
        }

        [BindProperty]
        public ElderDetails Elder { get; set; }

        #region ���� Docx
        public async Task<IActionResult> OnPostDownloadDocx()
        {
            var query = await _fileService.GetElderDocxById(Elder.Id);
            if (query.IsSuccess)
            {
                return File(query.Value, query.MimeType, query.ValueName);
            }
            else
            {
                ModelState.AddModelError(string.Empty, query.Error);
                return await OnGet(Elder.Id);
            }
        }
        #endregion

        #region �������֤ͼƬ
        public async Task<IActionResult> OnPostDownloadIdPhoto()
        {
            var query = await _fileService.GetElderPhotoPath(Elder.Id);
            if (query.IsSuccess)
            {
                return PhysicalFile(query.Value, query.MimeType, query.ValueName);
            }
            else
            {
                ModelState.AddModelError(string.Empty, query.Error);
                return await OnGet(Elder.Id);
            }
        }
        #endregion

        #region ɾ�� Elder and family
        public async Task<IActionResult> OnPostDeleteElder()
        {
            await _formService.DeleteElderById(Elder.Id);

            return RedirectToPage("Elder");
        }
        #endregion
    }
}
