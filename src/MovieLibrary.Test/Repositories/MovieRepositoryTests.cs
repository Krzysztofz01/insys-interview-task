using Moq;
using MovieLibrary.Data.Contracts;
using MovieLibrary.Data.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace MovieLibrary.Test.Repositories
{
    public class MovieRepositoryTests
    {
        [Fact]
        public async void GetMoviesAsyncShouldReturnMovies()
        {
            // Arrange
            var movieInMemoryDatabase = new Collection<Movie>
            {
                new Movie { Id = 1, Title = "Movie one" },
                new Movie { Id = 2, Title = "Movie two"},
                new Movie { Id = 3, Title = "Movie three" }
            };

            var repository = new Mock<IMovieRepository>();

            repository.Setup(r => r.GetMoviesAsync())
                .Returns(() => Task.FromResult(movieInMemoryDatabase.AsEnumerable()));

            var expectedCount = 3;

            // Act
            var actual = await repository.Object.GetMoviesAsync();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async void GetMoviesAsyncWithPredicateShouldReturnMovies()
        {
            // Arrange
            var movieInMemoryDatabase = new Collection<Movie>
            {
                new Movie { Id = 1, Title = "Movie one" },
                new Movie { Id = 2, Title = "Movie two"},
                new Movie { Id = 3, Title = "Movie three" }
            };

            var repository = new Mock<IMovieRepository>();

            repository.Setup(r => r.GetMoviesAsync(It.IsAny<Expression<Func<Movie, bool>>>()))
                .Returns((Expression<Func<Movie, bool>> p) => Task.FromResult(movieInMemoryDatabase.Where(p.Compile())));

            var expectedId = 1;
            var expectedCount = 1;

            // Act
            var actual = await repository.Object.GetMoviesAsync(e => e.Id == expectedId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actual.Count());
            Assert.Equal(expectedId, actual.Single().Id);
        }

        [Fact]
        public async void GetMovieByIdAsyncShouldReturnMovie()
        {
            // Arrange
            var movieInMemoryDatabase = new Collection<Movie>
            {
                new Movie { Id = 1, Title = "Movie one" },
                new Movie { Id = 2, Title = "Movie two"},
                new Movie { Id = 3, Title = "Movie three" }
            };

            var repository = new Mock<IMovieRepository>();

            repository.Setup(r => r.GetMovieByIdAsync(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(movieInMemoryDatabase.SingleOrDefault(b => b.Id == i)));

            var expectedTitle = "Movie one";
            var expectedId = 1;

            // Act
            var actual = await repository.Object.GetMovieByIdAsync(expectedId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedTitle, actual.Title);
        }

        [Fact]
        public async void InsertMovieAsyncShouldAddNewMovie()
        {
            // Arrange
            var movieInMemoryDatabase = new Collection<Movie>
            {
                new Movie { Id = 1, Title = "Movie one" },
                new Movie { Id = 2, Title = "Movie two"},
                new Movie { Id = 3, Title = "Movie three" }
            };

            var repository = new Mock<IMovieRepository>();

            repository.Setup(r => r.GetMovieByIdAsync(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(movieInMemoryDatabase.SingleOrDefault(b => b.Id == i)));

            repository.Setup(r => r.InsertMovieAsync(It.IsAny<Movie>()))
                .Returns((Movie m) =>
                {
                    movieInMemoryDatabase.Add(m);

                    return Task.CompletedTask;
                });

            var expectedTitle = "Movie four";
            var expectedId = 4;

            // Act
            await repository.Object.InsertMovieAsync(new Movie { Id = expectedId, Title = expectedTitle });

            var actual = await repository.Object.GetMovieByIdAsync(expectedId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedTitle, actual.Title);
        }

        [Fact]
        public async void DeleteMovieShouldRemoveMovie()
        {
            // Arrange
            var movieInMemoryDatabase = new Collection<Movie>
            {
                new Movie { Id = 1, Title = "Movie one" },
                new Movie { Id = 2, Title = "Movie two"},
                new Movie { Id = 3, Title = "Movie three" }
            };

            var repository = new Mock<IMovieRepository>();

            repository.Setup(r => r.GetMovieByIdAsync(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(movieInMemoryDatabase.SingleOrDefault(b => b.Id == i)));

            repository.Setup(r => r.DeleteMovie(It.IsAny<Movie>()))
                .Callback((Movie m) => movieInMemoryDatabase.Remove(m))
                .Verifiable();

            var expectedCount = 2;
            var expectedId = 1;

            // Act
            var movie = await repository.Object.GetMovieByIdAsync(expectedId);

            repository.Object.DeleteMovie(movie);

            // Assert
            Assert.Null(movieInMemoryDatabase.SingleOrDefault(m => m.Id == expectedId));
            Assert.Equal(expectedCount, movieInMemoryDatabase.Count);
        }
    }
}
