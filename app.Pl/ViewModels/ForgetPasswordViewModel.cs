using System.ComponentModel.DataAnnotations;

namespace app.Pl.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

    }
}
