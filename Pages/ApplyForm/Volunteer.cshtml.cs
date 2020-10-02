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
            //��֤��
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError(string.Empty, "��δ���� �� ���");
                return Page();
            }

            //��֤ͼƬ��Ч
            if (IdPhoto.Length <= 0)
            {
                ModelState.AddModelError(string.Empty, "���֤ͼƬ��Ч,������ѡ��");
                return Page();
            }

            //_volunteerForm ��ֵ
            _volunteerForm.Volunteer = Volunteer;
            _volunteerForm.Family = Family;
            _volunteerForm.IdPhoto = IdPhoto;

            //д�����ݿ�
            var query = await _dbService.SaveVolunteerToDb(_volunteerForm);

            //д�������Ƿ�ɹ�
            if (query.IsSuccess)
            {
                //�����ʼ�
                await _mailService.SendMail("�����幤�����", $"����: {Volunteer.Name}");
                return RedirectToPage("FormResult", new { Result = "�ύ�ɹ���лл!" });
            }
            else
            {
                return RedirectToPage("FormResult", new { Result = query.Error });
            }
        }
      
    }
}
