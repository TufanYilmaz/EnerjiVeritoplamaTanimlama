using System.ComponentModel.DataAnnotations;

namespace SuperFilm.Enerji.WebUI.ViewModels.AccountViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mevcut Şifre boş olamaz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni Şifre boş olamaz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni Şifre Tekrar boş olamaz.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
