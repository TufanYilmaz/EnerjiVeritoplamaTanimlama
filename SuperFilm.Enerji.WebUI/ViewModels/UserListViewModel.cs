namespace SuperFilm.Enerji.WebUI.ViewModels.UserViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> SelectedRoles { get; set; } = new List<string>();  // ✅ Buraya eklenecek
    }
}
