using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VihoTask.Models.ModelChat;

namespace VihoTask.Models
{
    public class VUser : IdentityUser
    {
        public VUser() : base()
        {
            Chats = new List<ChatUser>();
        }
        public ICollection<ChatUser> Chats { get; set; }

        public ICollection<VTask> Tasks { get; set; }

        public byte[] VUserPhoto { get; set; }

        public string VUserPost { get; set; }

        public string VUserAbout { get; set; }

        public byte[] VUserBanner { get; set; }

    }
}
