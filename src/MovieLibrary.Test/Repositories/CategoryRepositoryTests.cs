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
    public class CategoryRepositoryTests
    {
        [Fact]
        public async void GetCategoriesAsyncShouldReturnCategories()
        {
            // Arrange
            var categoryInMemoryDatabase = new Collection<Category>
            {
                new Category { Id = 1, Name = "Category one" },
                new Category { Id = 2, Name = "Category two"},
                new Category { Id = 3, Name = "Category three" }
            };

            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetCategoriesAsync())
                .Returns(() => Task.FromResult(categoryInMemoryDatabase.AsEnumerable()));

            var expectedCount = 3;

            // Act
            var actual = await repository.Object.GetCategoriesAsync();

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actual.Count());
        }

        [Fact]
        public async void GetCategoriesAsyncWithPredicateShouldReturnCategories()
        {
            // Arrange
            var categoryInMemoryDatabase = new Collection<Category>
            {
                new Category { Id = 1, Name = "Category one" },
                new Category { Id = 2, Name = "Category two"},
                new Category { Id = 3, Name = "Category three" }
            };

            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .Returns((Expression<Func<Category, bool>> p) => Task.FromResult(categoryInMemoryDatabase.Where(p.Compile())));

            var expectedId = 1;
            var expectedCount = 1;

            // Act
            var actual = await repository.Object.GetCategoriesAsync(e => e.Id == expectedId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedCount, actual.Count());
            Assert.Equal(expectedId, actual.Single().Id);
        }

        [Fact]
        public async void GetCategoryByIdAsyncShouldReturnCategory()
        {
            // Arrange
            var categoryInMemoryDatabase = new Collection<Category>
            {
                new Category { Id = 1, Name = "Category one" },
                new Category { Id = 2, Name = "Category two"},
                new Category { Id = 3, Name = "Category three" }
            };

            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(categoryInMemoryDatabase.SingleOrDefault(b => b.Id == i)));

            var expectedName = "Category one";
            var expectedId = 1;

            // Act
            var actual = await repository.Object.GetCategoryByIdAsync(expectedId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedName, actual.Name);
        }

        [Fact]
        public async void InsertCategoryAsyncShouldAddNewCategory()
        {
            // Arrange
            var categoryInMemoryDatabase = new Collection<Category>
            {
                new Category { Id = 1, Name = "Category one" },
                new Category { Id = 2, Name = "Category two"},
                new Category { Id = 3, Name = "Category three" }
            };

            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(categoryInMemoryDatabase.SingleOrDefault(b => b.Id == i)));

            repository.Setup(r => r.InsertCategoryAsync(It.IsAny<Category>()))
                .Returns((Category c) =>
                {
                    categoryInMemoryDatabase.Add(c);

                    return Task.CompletedTask;
                });

            var expectedName = "Category four";
            var expectedId = 4;

            // Act
            await repository.Object.InsertCategoryAsync(new Category { Id = expectedId, Name = expectedName });

            var actual = await repository.Object.GetCategoryByIdAsync(expectedId);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedName, actual.Name);
        }

        [Fact]
        public async void DeleteCategoryShouldRemoveCateogry()
        {
            // Arrange
            var categoryInMemoryDatabase = new Collection<Category>
            {
                new Category { Id = 1, Name = "Category one" },
                new Category { Id = 2, Name = "Category two"},
                new Category { Id = 3, Name = "Category three" }
            };

            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetCategoryByIdAsync(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult(categoryInMemoryDatabase.SingleOrDefault(b => b.Id == i)));

            repository.Setup(r => r.DeleteCategory(It.IsAny<Category>()))
                .Callback((Category c) => categoryInMemoryDatabase.Remove(c))
                .Verifiable();

            var expectedCount = 2;
            var expectedId = 1;

            // Act
            var category = await repository.Object.GetCategoryByIdAsync(expectedId);

            repository.Object.DeleteCategory(category);

            // Assert
            Assert.Null(categoryInMemoryDatabase.SingleOrDefault(m => m.Id == expectedId));
            Assert.Equal(expectedCount, categoryInMemoryDatabase.Count);
        }
    }
}
