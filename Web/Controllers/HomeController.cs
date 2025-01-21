using Application.DTOs;
using Application.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _path = "C:\\FileManagementSystem\\";

        private readonly IUser _user;
        private readonly ILogger<HomeController> _logger;
        private readonly IUserFileService _userFile;

        public HomeController(
            ILogger<HomeController> logger,
            IUserFileService userFile,
            IUser user)
        {
            _logger = logger;
            _userFile = userFile;
            _user = user;
        }
        private string GetUserFilesPath(string userId)
        {
            var _filePath = Path.Combine($"{_path}{userId}");
            return _filePath;
        }
        public IActionResult Index(string CurrentPath = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentPath = CurrentPath;

                var uploadsPath = Path.Combine(GetUserFilesPath(_user.Id));

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                List<string> directories;
                if (CurrentPath.Length > 0)
                {
                    directories = Directory.GetDirectories($"{uploadsPath}\\{CurrentPath}").ToList();
                }
                else
                {
                    directories = Directory.GetDirectories($"{uploadsPath}").ToList();
                }
                var FolderNames = new List<string>();
                foreach (var item in directories)
                {
                    FolderNames.Add(Path.GetFileName(item));
                }

                ViewBag.directories = FolderNames;


                var list = _userFile.GetUserFileWithDetailsByUserId(_user.Id);
                ViewBag.AllFileShareWithMe = _userFile.AllFileShareWithMe(_user.Id);

                return View(list);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(string CurrentPath = "")
        {
            IFormFile file;
            if (Request.Form.Files.Count > 0)
            {
                file = Request.Form.Files[0];
                if (file.FileName.Length > 30 || file.FileName.Count(c => c == '.') > 1)
                {
                    return BadRequest("نام فایل نباید بیشتر از 30 کاراکتر باشد.");
                }
            }
            else
            {
                return BadRequest("فایلی ارسال نشده است.");
            }

            var uploadsPath = Path.Combine(GetUserFilesPath(_user.Id));

            uploadsPath = $"{uploadsPath}{CurrentPath}";

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _userFile.AddAsync(new UserFileDto()
            {
                UploadedById = _user.Id,
                Name = file.FileName,
                FilePath = CurrentPath,
                Size=file.Length,
            });

            return Ok(new { Message = "فایل با موفقیت آپلود شد." });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
