using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VihoTask.Infrastructure;
using VihoTask.Models;
using VihoTask.Models.ViewModels;

namespace VihoTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<VUser> _userManager;
        private readonly SignInManager<VUser> _signInManager;
        private readonly DataContext _context; 

        public AccountController(UserManager<VUser> userManager, SignInManager<VUser> signInManager, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = dataContext;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user, IFormFile Photo, IFormFile Banner)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0 && Banner != null && Banner.Length > 0)
                {
                    // Прочитать содержимое IFormFile и сохранить в массив байтов
                    byte[] photoBytes;
                    using (MemoryStream photoStream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(photoStream);
                        photoBytes = photoStream.ToArray();
                    }

                    byte[] bannerBytes;
                    using (MemoryStream bannerStream = new MemoryStream())
                    {
                        await Banner.CopyToAsync(bannerStream);
                        bannerBytes = bannerStream.ToArray();
                    }

                    VUser newUser = new VUser
                    {
                        UserName = user.UserName,
                        VUserBanner = bannerBytes,
                        VUserPost = user.PostUser,
                        VUserAbout = user.UserAbout,
                        Email = user.Email,
                        VUserPhoto = photoBytes
                    };

                    IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        if (user.UserName == "Admin")
                        {
                            await _userManager.AddToRoleAsync(newUser, "Admin");
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(newUser, "User");
                        }
                        await _signInManager.SignInAsync(newUser, isPersistent: false); // Вход пользователя
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Выберите фотографии");
                }
            }

            return View(user);
        }


        public async Task<IActionResult> Edit(IFormFile Photo, IFormFile Banner)
        {

            // Получите текущего пользователя
            var currentUser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            if (currentUser != null)
            {
                UserModel userModel = new UserModel
                {
                    UserName = currentUser.UserName,
                    Email = currentUser.Email,
                    PostUser = currentUser.VUserPost,
                    UserAbout = currentUser.VUserAbout, 
                    Banner = currentUser.VUserBanner,
                    Photo = currentUser.VUserPhoto,
                };

                return View(userModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserModel user, IFormFile Photo, IFormFile Banner)
        {
                var currentUser = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                if (Photo != null && Photo.Length > 0)
                {
                    byte[] photoBytes;
                    using (MemoryStream photoStream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(photoStream);
                        photoBytes = photoStream.ToArray();
                    }

                    currentUser.VUserPhoto = photoBytes;
                }

                if (Banner != null && Banner.Length > 0)
                {
                    byte[] bannerBytes;
                    using (MemoryStream bannerStream = new MemoryStream())
                    {
                        await Banner.CopyToAsync(bannerStream);
                        bannerBytes = bannerStream.ToArray();
                    }

                    currentUser.VUserBanner = bannerBytes;
                }

                currentUser.Email = user.Email;
                currentUser.VUserPost = user.PostUser;
                currentUser.VUserAbout = user.UserAbout;
                currentUser.NormalizedEmail = user.Email.ToUpper();

                IdentityResult result = await _userManager.UpdateAsync(currentUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
        }


            public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var user = await _userManager.FindByNameAsync(Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Login", "Account");
        }




        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
