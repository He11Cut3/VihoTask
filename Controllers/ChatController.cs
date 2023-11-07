using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using VihoTask.Hubs;
using VihoTask.Infrastructure;
using VihoTask.Infrastructure.Repository;
using VihoTask.Models;

namespace VihoTask.Controllers
{
    [Authorize]
    public class ChatController : BaseController
    {
        private IChatRepository _repo;
        public ChatController(IChatRepository repo) => _repo = repo;
        public IActionResult Index()
        {
            var chats = _repo.GetChats(GetUserId());

            return View(chats);
        }

        public IActionResult Find([FromServices] DataContext ctx)
        {
            var users = ctx.Users
                .Where(x => x.Id != User.GetUserId())
                .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            var chats = _repo.GetPrivateChats(GetUserId());

            return View(chats);
        }

        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var id = await _repo.CreatePrivateRoom(GetUserId(), userId);

            return RedirectToAction("Chat", new { id });
        }


        public IActionResult Chat(int id)
        {
            return View(_repo.GetChat(id));
        }

        public async Task<IActionResult> CreateRoom()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _repo.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _repo.JoinRoom(id, GetUserId());

            return RedirectToAction("Index", "Chat", new { id = id });
        }

        public async Task<IActionResult> SendMessage(
            int roomId,
            string message,
            [FromServices] IHubContext<ChatHub> chat)
        {
            var Message = await _repo.CreateMessage(roomId, message, User.Identity.Name);

            await chat.Clients.Group(roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }


    }
}
