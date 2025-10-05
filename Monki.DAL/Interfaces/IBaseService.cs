namespace Monki.DAL.Interfaces
{
	public interface IBaseService<T>
	{
		Task AddAsync(T item);
		Task Delete(T item);
		IEnumerable<T> GetAll();
		Task<T> GetById(int id);
		void UpdateModel(T item);
	}
}
