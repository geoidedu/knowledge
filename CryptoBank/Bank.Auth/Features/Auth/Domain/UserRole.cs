using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Auth.Features.Auth.Domain
{
    public class UserRole
    {
        public int Id { get; set; }
        public Role Role { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } = null!;
        public User User { get; set; }
    }


    public enum Role
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        User,
        /// <summary>
        /// Аналитик
        /// </summary>
        Analist,
        /// <summary>
        /// Администратор
        /// </summary>
        Administrator
    }
}