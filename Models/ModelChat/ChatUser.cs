namespace VihoTask.Models.ModelChat
{
    public class ChatUser
    {
        public string UserId { get; set; }
        public VUser User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public UserRole Role { get; set; }
    }
}