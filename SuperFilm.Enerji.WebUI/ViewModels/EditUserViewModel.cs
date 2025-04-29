namespace SuperFilm.Enerji.WebUI.ViewModels.UserViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> SelectedRoles { get; set; } = new List<string>(); 
    }
}
