using SapWebServices.Model;

namespace SapWebServices.Helpers
{
    public interface IDB2Helper
    {
        public EnerjiModel GetEnerji(string? UretimYeri, DateTime startDateTime, DateTime endDateTime, HelperPlants? helperPlants);
        public EnerjiModel GetEnerji(EnerjiRequestModel enerjiRequest);

    }
}
