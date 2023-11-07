namespace VihoTask.Models
{
    
    public interface IUserRepository
    {
        VUser GetUserById(string userId); // Получение пользователя по идентификатору
        List<VUser> GetAllUsers(); // Получение всех пользователей (если требуется)
        // Другие методы, например, добавление, обновление и удаление пользователей
    }
}
