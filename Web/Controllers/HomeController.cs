using Application.DTOs;
using Application.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Diagnostics;
using Web.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _path = "C:\\FileManagementSystem\\";

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUser _user;
        private readonly ILogger<HomeController> _logger;
        private readonly IUserFileService _userFile;
        private readonly IFileShareService _fileShare;

        public HomeController(
            ILogger<HomeController> logger,
            IUserFileService userFile,
            IFileShareService fileShare,
            IUser user,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userFile = userFile;
            _user = user;
            _userManager = userManager;
            _fileShare = fileShare;
        }
        private string GetUserFilesPath(string userId)
        {
            var _filePath = Path.Combine($"{_path}{userId}");
            return _filePath;
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<string>());

            var users = _userManager.Users
                .Where(user => user.Id != _user.Id && user.Email.Contains(query))
                .Select(user => user.Email)
                .ToList();


            if (!users.Any())
                return NotFound("user not found!");

            return Ok(users);
        }

        public async Task<IActionResult> Index(string CurrentPath = "")
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


                var list = await _userFile.GetUserFileWithDetailsByUserId(_user.Id);

                list = list.Where(c => (string.IsNullOrEmpty(c.FilePath) && string.IsNullOrWhiteSpace(CurrentPath)) || c.FilePath == CurrentPath).OrderByDescending(c => c.Id);

                ViewBag.AllFileShareWithMe = (await _userFile.AllFileShareWithMe(_user.Id)).ToList();

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

            await _userFile.NewAsync(new UserFileDto()
            {
                UploadedById = _user.Id,
                Name = file.FileName,
                FilePath = CurrentPath,
                Size = file.Length,
            });

            return Ok(new { Message = "فایل با موفقیت آپلود شد." });
        }

        [HttpPost]
        public IActionResult NewFolder(FolderNameDto model, string? CurrentPath)
        {
            if (ModelState.IsValid)
            {
                var uploadsPath = $"{GetUserFilesPath(_user.Id)}" +
                    $"{(
                        (CurrentPath != null && CurrentPath.Any()) ?
                        $"\\{CurrentPath}\\" :
                        "")}" +
                    $"\\{model.FolderName}";

                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                return RedirectToAction(nameof(Index), new { CurrentPath = CurrentPath });
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteFolder(string FolderName, string CurrentPath = "")
        {
            //CurrentPath = string.IsNullOrEmpty(CurrentPath) ? "\\" : CurrentPath;


            var uploadsPath = Path.Combine(GetUserFilesPath(_user.Id));

            uploadsPath = $"{uploadsPath}{CurrentPath}\\{FolderName}";

            var list = await _userFile.GetUserFileWithDetailsByUserId(_user.Id);

            var files = list.Where(c =>
                c.UploadedById == _user.Id &&
                c.FilePath != null &&
                (c.FilePath == $"{CurrentPath}\\{FolderName}" || c.FilePath.StartsWith($"{CurrentPath}\\{FolderName}\\"))
            ).ToList();


            foreach (var file in files)
            {
                await DeleteUserFileAsync(file);
            }

            if (Directory.Exists(uploadsPath))
            {
                Directory.Delete(uploadsPath, true);
            }

            return RedirectToAction("Index", new { CurrentPath = CurrentPath });
        }

        public async Task<IActionResult> Delete(Guid Id)
        {

            var userFile = await _userFile.GetFileWithDetails(Id);


            if (
                userFile.UploadedById == _user.Id)
            {

                await DeleteUserFileAsync(userFile);

                return RedirectToAction(nameof(Index), new { CurrentPath = userFile.FilePath });
            }
            else
            {
                return Content("Fail");
            }
        }
        private async Task DeleteUserFileAsync(UserFileDto userFile)
        {
            var uploadsPath = Path.Combine(GetUserFilesPath(userFile.UploadedById));

            string WithFolderPath = uploadsPath;

            if (!string.IsNullOrEmpty(userFile.FilePath))
            {
                WithFolderPath = $"{uploadsPath}{userFile.FilePath}";
            }

            var filePath = Path.Combine(WithFolderPath, userFile.Name);

            if (!System.IO.File.Exists(filePath))
                return;


            var fileShareList = userFile.SharedWithUsers.ToList();
            foreach (var item in fileShareList)
            {
                await _fileShare.Delete(item);
            }
            await _userFile.Delete(userFile);

            System.IO.File.Delete(filePath);
        }


        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            //UserFile userFile = new UserFile();

            var userFile = await _userFile.GetFileWithDetails(Id);

            if (userFile.UploadedById == _user.Id || userFile.IsPublic || userFile.SharedWithUsers.Any(c => c.SharedWithUserId == _user.Id))
            {

                var uploadsPath = Path.Combine(GetUserFilesPath(userFile.UploadedById));

                string WithholderPath = uploadsPath;

                if (!string.IsNullOrEmpty(userFile.FilePath))
                {
                    WithholderPath = $"{uploadsPath}{userFile.FilePath}";
                }

                var filePath = $"{WithholderPath}\\{userFile.Name}";

                if (!System.IO.File.Exists(filePath))
                    return NotFound("Not found.");

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", userFile.Name);
            }
            else
            {
                return Content("Fail");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Share(string FileId, string Email, bool IsPublic, string? CurrentPath)
        {
            var file = await _userFile.GetFileWithDetails(new Guid(FileId));
            var user = _userManager.Users
                           .FirstOrDefault(user => user.Email == Email);

            if (user != null)
            {

                if (!file.SharedWithUsers.Any(c => c.SharedWithUserId == user.Id))
                    await _fileShare.NewFileShare(new FileShareDto()
                    {
                        SharedWithUserId = user.Id,
                        UserFileId = file.Id
                    });

            }

            if (file.IsPublic != IsPublic)
            {
                file.IsPublic = IsPublic;
                await _userFile.ModifyAsync(file);
            }



            return Json(new { status = "ok" });


        }
        [HttpPost]
        public async Task<ActionResult> DeletePerson(string Id)
        {
            var list = await _userFile.GetUserFileWithDetailsByUserId(_user.Id);
            var fileShare = list.SelectMany(c => c.SharedWithUsers).FirstOrDefault(c => c.Id == new Guid(Id));


            if (fileShare != null)
            {
                await _fileShare.Delete(fileShare);
                return Json(new { status = "ok" });

            }
            return Json(new { status = "fail" });
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
