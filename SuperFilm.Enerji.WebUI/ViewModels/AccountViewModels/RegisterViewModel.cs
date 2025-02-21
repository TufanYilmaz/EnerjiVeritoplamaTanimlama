using System.ComponentModel.DataAnnotations;

namespace SuperFilm.Enerji.WebUI.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "İsim Gereklidir.")]
        [Display(Name = "İsim")]
        public string Name { get; set; }
        [Required(ErrorMessage = "E-Posta Gereklidir.")]
        [EmailAddress]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Gereklidir.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Parola {0} uzunluk aralığı {2}-{1} aralığında olmalıdır")]
        [Compare("ConfirmPassword", ErrorMessage = "Parolalar Eşleşmiyor")]
        [Display(Name = "Parola")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Parola Tekrarı Gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola Tekrarı")]
        public string ConfirmPassword { get; set; }

    }
}
