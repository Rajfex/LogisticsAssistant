using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required]
    public string Name { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}