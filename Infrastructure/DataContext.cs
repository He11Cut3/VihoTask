using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VihoTask.Models;
using VihoTask.Models.ModelChat;

namespace VihoTask.Infrastructure
{
    public class DataContext : IdentityDbContext<VUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<VTask> VTasks { get; set; }

        public DbSet<VUser> VUsers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        public DbSet<VTaskContactUs> VTaskContactUss { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }
    }
}
