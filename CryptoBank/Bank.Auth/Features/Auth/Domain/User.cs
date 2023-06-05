using System.ComponentModel.DataAnnotations;

namespace Bank.Auth.Features.Auth.Domain;

public class User
{
    [Key]
    public string Id { get; set; } = null!;

    [StringLength(maximumLength: 25, MinimumLength = 5, ErrorMessage = "Мах длина страки 20")]
    public string UserName { get; set; } = null!;

    [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Мах длина страки 20")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Дата рождение
    /// </summary>
    public string? DateOfBirth { get; set; }
    /// <summary>
    /// Дата регистрация
    /// </summary>
    public string DateOfRegister { get; set; } = null!;
}


