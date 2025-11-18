using System.ComponentModel.DataAnnotations;

namespace app.Pl.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "NewPassword Is Requird")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password dosnot match")]
        public string ConfirmPassword { get; set; }

    }
}
