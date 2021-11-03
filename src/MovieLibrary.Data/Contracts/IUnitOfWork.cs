using System.Threading.Tasks;

namespace MovieLibrary.Data.Contracts
{
    public interface IUnitOfWork
    {
        IMovieRepository MovieRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task SaveAsync(); 
    }
}
