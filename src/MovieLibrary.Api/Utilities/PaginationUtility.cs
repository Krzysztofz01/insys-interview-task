using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Api.Utilities
{
    public static class PaginationUtility
    {
        private static readonly int _maxElementsOnPage = 4;

        public static IEnumerable<TEntity> Paginate<TEntity>(IEnumerable<TEntity> collection, int page) where TEntity : class
        {
            if (page < 1)
                throw new ArgumentException("Invalid page number");

            return collection.Skip((page - 1) * _maxElementsOnPage).Take(_maxElementsOnPage);
        }
    }
}
