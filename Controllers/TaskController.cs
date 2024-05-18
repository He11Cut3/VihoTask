using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VihoTask.Infrastructure;
using VihoTask.Models;

namespace VihoTask.Controllers
{
    [Authorize]

    public class TaskController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<VUser> _userManager;

        public TaskController(DataContext context, UserManager<VUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var tasks = await _context.VTasks
                    .Include(t => t.VUser)
                    .ToListAsync();

                return View(tasks);
            }
            else
            {
                var UserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var tasks = await _context.VTasks
                    .Include(t => t.VUser)
                    .Where(t => t.VUserID == UserID)
                    .ToListAsync();
                return View(tasks);
            }

            
        }

        public IActionResult Calendate()
        {
            return View();
        }

        public IActionResult GetTasks()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            // Если текущий пользователь администратор, получаем все задачи
            if (User.IsInRole("Admin"))
            {
                var tasks = _context.VTasks.ToList();

                var calendarEvents = tasks.Select(task => new
                {
                    id = task.VTaskID,
                    title = task.VTaskName,
                    start = task.VTaskDateStart,
                    end = task.VTaskDateEnd,
                    description = task.VTaskDescription,
                    priority = task.VTaskPriority,
                });

                return Json(calendarEvents);
            }
            else // Если текущий пользователь не администратор, получаем задачи только для этого пользователя
            {
                var tasks = _context.VTasks.Where(task => task.VUserID == currentUser.Id).ToList();

                var calendarEvents = tasks.Select(task => new
                {
                    id = task.VTaskID,
                    title = task.VTaskName,
                    start = task.VTaskDateStart,
                    end = task.VTaskDateEnd,
                    description = task.VTaskDescription,
                    priority = task.VTaskPriority,
                });

                return Json(calendarEvents);
            }
        }




        [Authorize(Roles = "Admin")]
        // Создание новой задачи (GET)
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VTask task)
        {
            if (task.FileUpdate != null && task.FileUpdate.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await task.FileUpdate.CopyToAsync(memoryStream);
                    task.VTaskFile = memoryStream.ToArray();
                    var fileInfo = new FileInfo(task.FileUpdate.FileName);
                    task.VTaskFileExtension = fileInfo.Extension.TrimStart('.');
                }
            }

            if (!string.IsNullOrEmpty(task.VUserID))
            {
                var selectedUser = await _userManager.FindByIdAsync(task.VUserID);

                if (selectedUser != null)
                {
                    task.VUser = selectedUser;
                }
                else
                {
                    ModelState.AddModelError("VUserID", "Выберите существующего пользователя.");
                    return View(task); // Return the view with the current task model to show validation errors
                }
            }
            else
            {
                ModelState.AddModelError("VUserID", "Необходимо указать пользователя.");
                return View(task); // Return the view with the current task model to show validation errors
            }

            if (ModelState.IsValid)
            {
                _context.VTasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(task); // Return the view with the current task model to show validation errors
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long id)
        {
            VTask task =  await _context.VTasks.FindAsync(id);

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(VTask editedTask, int? id)
        {
            if (ModelState.IsValid)
            {
                var originalTask = await _context.VTasks.FirstOrDefaultAsync(p => p.VTaskID == id);

                // Обновляем свойства задачи
                originalTask.VTaskName = editedTask.VTaskName;
                originalTask.VTaskDescription = editedTask.VTaskDescription;
                originalTask.VTaskStatus = editedTask.VTaskStatus;
                originalTask.VTaskType = editedTask.VTaskType;
                originalTask.VTaskDateStart = editedTask.VTaskDateStart;
                originalTask.VTaskDateEnd = editedTask.VTaskDateEnd;
                originalTask.VTaskPriority = editedTask.VTaskPriority;
                originalTask.VUserID = editedTask.VUserID;

                // Если пользователь прикрепил новый файл, обновляем его
                if (editedTask.FileUpdate != null && editedTask.FileUpdate.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await editedTask.FileUpdate.CopyToAsync(memoryStream);
                        originalTask.VTaskFile = memoryStream.ToArray();
                        originalTask.VTaskFileExtension = Path.GetExtension(editedTask.FileUpdate.FileName).TrimStart('.');
                    }
                }

                _context.VTasks.Update(originalTask);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(editedTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(long id, string newStatus)
        {
            var task = _context.VTasks.FirstOrDefault(t => t.VTaskID == id);

            task.VTaskStatus = newStatus;
            _context.VTasks.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            VTask product = await _context.VTasks.FindAsync(id);

            _context.VTasks.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.VTasks.FirstOrDefaultAsync(t => t.VTaskID == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


    }
}
