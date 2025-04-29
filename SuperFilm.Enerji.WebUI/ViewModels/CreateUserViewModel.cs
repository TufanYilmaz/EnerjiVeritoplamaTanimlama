using System.ComponentModel.DataAnnotations;

namespace SuperFilm.Enerji.WebUI.ViewModels.UserViewModels
{
    public class CreateUserViewModel
    {
        public string Name { get; set; }
        [Required(ErrorMessage = "E-Posta Gereklidir.")]
        [EmailAddress]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Gereklidir.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Parola {0} uzunluk aralığı {2}-{1} aralığında olmalıdır")]
        public string Password { get; set; }
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
