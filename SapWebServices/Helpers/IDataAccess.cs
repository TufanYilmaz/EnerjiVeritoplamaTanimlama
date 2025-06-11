using SapWebServices.Model;

namespace SapWebServices.Helpers
{
	public interface IDataAccess
	{
		public  Task<IEnumerable<EnerjiRequestAdvanceModelDb>> GetAsycn();
		public Task<EnerjiRequestAdvanceModelDb> GetAsycn(EnerjiRequestAdvanceModel model);
		public EnerjiRequestAdvanceModelDb Get(EnerjiRequestAdvanceModel model);

		public  Task<int> AddAsync(EnerjiRequestAdvanceModelDb model);
		public  int Add(EnerjiRequestAdvanceModelDb model);
	}
}
