using MovieLibrary.Data.Contracts;
using System;
using System.Threading.Tasks;

namespace MovieLibrary.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieLibraryContext _context;
        
        private IMovieRepository _movieRepository;
        private ICategoryRepository _categoryRepository;

        public UnitOfWork(MovieLibraryContext movieLibraryContext)
        {
            _context = movieLibraryContext ??
                throw new ArgumentNullException(nameof(movieLibraryContext));
        }

        public IMovieRepository MovieRepository
        {
            get
            {
                if (_movieRepository is null) _movieRepository = new MovieRepository(_context);

                return _movieRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository is null) _categoryRepository = new CategoryRepository(_context);

                return _categoryRepository;
            }
        }

        public async Task SaveAsync()
        {
            _ = await _context.SaveChangesAsync();
        }
    }
}
