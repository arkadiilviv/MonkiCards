using Monki.DAL.Models;

namespace Monki.DAL.Interfaces
{
	public interface IBaseService<T>
	{
		Task<T> AddAsync(T item);
		Task Delete(T item);
		IEnumerable<T> GetAll();
		T GetById(int id);
		Task UpdateModelAsync(T item);
	}
}
