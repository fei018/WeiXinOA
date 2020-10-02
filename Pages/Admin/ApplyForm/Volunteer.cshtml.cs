using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeiXinOA.Models.ApplyForm;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Pages.Admin.ApplyForm
{
    public class VolunteerModel : PageModel
    {
        private readonly WXFormService _dbService;
        private readonly WXFileService _fileService;

        public VolunteerModel(WXFormService dbService, WXFileService fileService)
        {
            this._dbService = dbService;
            this._fileService = fileService;
        }

        public void OnGet()
        {

        }

        #region 分页获取表格数据
        public async Task<IActionResult> OnGetQueryAllPaged(int? pages, int? limit)
        {
            int pageNumber = pages ?? 1;
            pageNumber = pageNumber < 0 ? 1 : pageNumber;

            int pageSize = limit ?? 10;
            pageSize = pageSize < 0 ? 10 : pageSize;

            var query = await _dbService.GetAllVolunteerPaged(pageNumber, pageSize);
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

        #region 查询数据 by name
        public async Task<IActionResult> OnPostQueryByName(string queryName)
        {
            var query = await _dbService.GetVolunteerByName(queryName);
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

        #region 下载 Docx
        public async Task<IActionResult> OnPostDownloadDocx(int? downloadId)
        {
            if (!downloadId.HasValue) return null;
            int id = downloadId.Value;
            var query = await _fileService.GetVolunteerDocxById(id);
            if (query.IsSuccess)
            {
                //return new PhysicalFileResult(query.Value, "application/msword");               
                return File(query.Value, "application/msword", query.ValueName);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 下载身份证图片
        public async Task<IActionResult> OnPostDownloadIdPhoto(int? downloadId, int? checkId)
        {
            if (!downloadId.HasValue)
            {
                //downloadId 没传值，就是先检查 数据是否有效
                int cid = checkId.Value;
                var query = await _fileService.GetVolunteerPhotoPath(cid);
                if (!query.IsSuccess)
                {
                    return new JsonResult(new { code = 400, msg = query.Error });
                }
                else
                {
                    return new JsonResult(new { code = 200 });
                }
            }
            else
            {
                // dowloadId 有效，开始下载图片
                int did = downloadId.Value;
                var query = await _fileService.GetVolunteerPhotoPath(did);
                return PhysicalFile(query.Value, query.MimeType, query.ValueName);
            }
        }
        #endregion

    }
}
