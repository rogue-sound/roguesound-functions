using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RogueSound.Common.Sorting
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> AddSort<T>(this IQueryable<T> queryable, SortModel sortModel)
            where T : class
        {
            if (sortModel == null || string.IsNullOrEmpty(sortModel?.Property)) return queryable;

            var lambda = GenerateOrderByLambda<T>(sortModel.Property);

            queryable = sortModel.SortDirection == SortDirection.Ascending
                ? queryable.OrderBy(lambda)
                : queryable.OrderByDescending(lambda);

            return queryable;
        }

        private static Expression<Func<T, object>> GenerateOrderByLambda<T>(string property)
        {
            var param = Expression.Parameter(typeof(T));
            var prop = Expression.Property(param, property);

            var expAsObject = Expression.Convert(prop, typeof(object));

            return Expression.Lambda<Func<T, object>>(expAsObject, param);
        }
    }
}