﻿using Microsoft.AspNetCore.SignalR;

namespace VihoTask.Hubs
{
    
    public class ChatHub : Hub
    {
        public Task JoinRoom(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public Task LeaveRoom(string roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }
    }
}
