using System.ComponentModel.DataAnnotations;

namespace ZMA.Contracts;

public record RegistrationReq(
    [Required]string Email, 
    [Required]string Username,
    [Required]string Name,
    [Required]string Password);