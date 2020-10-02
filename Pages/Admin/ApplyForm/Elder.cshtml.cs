using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.Admin.ApplyForm
{
    public class ElderModel : PageModel
    {
        private readonly WXFormService _dbService;
        private readonly WXFileService _fileService;

        public ElderModel(WXFormService dbService, WXFileService fileService)
        {
            this._dbService = dbService;
            this._fileService = fileService;
        }

        public void OnGet()
        {
        }

        #region ��ҳ��ȡ�������
        public async Task<IActionResult> OnGetQueryAllPaged(int? pages, int? limit)
        {
            int pageNumber = pages ?? 1;
            pageNumber = pageNumber < 0 ? 1 : pageNumber;

            int pageSize = limit ?? 10;
            pageSize = pageSize < 0 ? 10 : pageSize;

            var query = await _dbService.GetAllElderPaged(pageNumber, pageSize);
            if (query.IsSuccess)
            {
                return new JsonResult(query.GetLayuiTableJsonData());
            }
            else
            {
                return new JsonResult(query.GetLayuiTableJsonData(query.Error));
            }
        }
        #endregion

        #region ��ѯ���� by name
        public async Task<IActionResult> OnPostQueryByName(string QueryName)
        {
            var query = await _dbService.GetElderByName(QueryName);
            if (query.IsSuccess)
            {
                return new JsonResult(query.GetLayuiTableJsonData());
            }
            else
            {
                return null;
            }
        }
        #endregion

        //#region ���� Docx
        //public async Task<IActionResult> OnPostDownloadDocx(int? downloadId)
        //{
        //    if (!downloadId.HasValue) return null;
        //    int id = downloadId.Value;
        //    var query = await _fileService.GetElderDocxById(id);
        //    if (query.IsSuccess)
        //    {
        //        return File(query.Value, "application/msword",query.ValueName);             
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //#endregion

        //#region �������֤ͼƬ
        //public async Task<IActionResult> OnPostDownloadIdPhoto(int? downloadId, int? checkId)
        //{
        //    if (!downloadId.HasValue)
        //    {
        //        //downloadId û��ֵ�������ȼ�� id�����Ƿ���Ч
        //        int cid = checkId.Value;
        //        var query = await _fileService.GetElderPhotoPath(cid);
        //        if (!query.IsSuccess)
        //        {
        //            return new JsonResult(new { code = 400, msg = query.Error });
        //        }
        //        else
        //        {
        //            return new JsonResult(new { code = 200 });
        //        }
        //    }
        //    else
        //    {
        //        // dowloadId ��Ч����ʼ����ͼƬ
        //        int did = downloadId.Value;
        //        var query = await _fileService.GetElderPhotoPath(did);
        //        return PhysicalFile(query.Value, query.MimeType, query.ValueName);
        //    }
        //}
        //#endregion
    }
}
