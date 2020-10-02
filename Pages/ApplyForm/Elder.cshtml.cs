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
                //ModelState.AddModelError(string.Empty, "��δ���� �� ���");
                return Page();
            }

            //��֤ͼƬ��С
            if (IdPhoto.Length <= 0)
            {
                ModelState.AddModelError(string.Empty, "���֤ͼƬ��Ч,������ѡ��");
                return Page();
            }

            //_elderForm��ֵ
            _elderForm.Elder = ElderDetails;
            _elderForm.Family1 = ElderFamily1;
            _elderForm.Family2 = ElderFamily2;
            _elderForm.IdPhoto = IdPhoto;

            //д�����ݿ�
            var query = await _dbService.SaveElderToDb(_elderForm);

            //д���Ƿ�ɹ�
            if (query.IsSuccess)
            {
                //�����ʼ�֪ͨ
                await _mailService.SendMail("�������������", $"����: {ElderDetails.Name}");
                return RedirectToPage("FormResult", new { Result = "�ύ�ɹ���лл!" });
            }
            else
            {
                return RedirectToPage("FormResult", new { Result = query.Error });
            }
        }
    }
}
