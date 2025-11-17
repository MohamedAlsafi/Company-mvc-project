using System.ComponentModel.DataAnnotations;

namespace app.Pl.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="FNmae Is Requird")]
        public string FName { get; set; }

        [Required(ErrorMessage = "LNmae Is Requird")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email Is Requird")]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Is Requird")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="password dosnot match")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
