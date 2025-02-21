using System.ComponentModel.DataAnnotations;

namespace SuperFilm.Enerji.WebUI.ViewModels.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "E-Posta Gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Gereklidir.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Parola {0} uzunluk aralığı {2}-{1} aralığında olmalıdır")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Parolalar Eşleşmiyor")]
        [Display(Name = "Yeni Parola")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Parola Tekrarı Gereklidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Parola Tekrarı")]
        public string ConfirmNewPassword { get; set; }
    }
}
