using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using VihoTask.Infrastructure;
using VihoTask.Models;

namespace VihoTask.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;

        private readonly DataContext _context;



        public IActionResult Return()
        {
            return Redirect("/");
        }

        public HomeController(IUserRepository userRepository, DataContext dataContext)
        {
            _userRepository = userRepository;
            _context = dataContext;
        }
        [Authorize]
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> ViewContact()
        {
            var contac = await _context.VTaskContactUss.ToListAsync();

            return View(contac);
        }


        public async Task<IActionResult> CreateForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateForm(VTaskContactUs vTaskContactUs)
        {
            _context.VTaskContactUss.Add(vTaskContactUs);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        [Authorize]
        public IActionResult UserInfo()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserModel userModel = GetUserModel(userId);

            return View("UserInfo", userModel);
        }

        private UserModel GetUserModel(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                VUser user = _userRepository.GetUserById(userId);

                if (user != null)
                {

                    return new UserModel
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        PostUser = user.VUserPost,
                        UserAbout = user.VUserAbout,
                        Photo = user.VUserPhoto,
                        Banner = user.VUserBanner
                    };
                }
            }

            // Если пользователь не найден, вернем пустую модель
            return new UserModel();
        }
    }
}
