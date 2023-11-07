using VihoTask.Infrastructure;

namespace VihoTask.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context; // Замените YourDbContext на ваш контекст базы данных

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public VUser GetUserById(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public List<VUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
