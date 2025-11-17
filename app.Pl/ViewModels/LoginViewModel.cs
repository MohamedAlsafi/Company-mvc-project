using System.ComponentModel.DataAnnotations;

namespace app.Pl.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
