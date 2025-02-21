using System.ComponentModel.DataAnnotations;

namespace SuperFilm.Enerji.WebUI.ViewModels.AccountViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "E-Posta Gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
