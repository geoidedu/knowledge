using System.ComponentModel.DataAnnotations;

namespace Bank.Auth.Features.Auth.Domain;

public class User
{
    [Key]
    public string Id { get; set; } = null!;

    [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Мах длина страки 20")]
    public string UserName { get; set; } = null!;

    [StringLength(maximumLength: 12, MinimumLength = 5, ErrorMessage = "Мах длина страки 20")]
    public string Password { get; set; } = null!;
}


