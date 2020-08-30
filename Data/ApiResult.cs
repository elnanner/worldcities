using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace WorldCities.Data
{
    public class ApiResult<T>
    {
        /// <summary>
        /// Private constructor calle by te CreateAsync method.
        /// </summary>
        private ApiResult(List<T> data, int count, int pageIndex, int pageSize, string sortColumn, string sortOrder)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)(Math.Ceiling(count / (double)pageSize));
            SortColumn = sortColumn;
            SortOrder = sortOrder;
        }
        #region Methods

        /// <summary>
        /// Pages a IQueryable source.
        /// </summary>
        /// <param name="source">An IQueryable source of generic
        /// type</param>
        /// <param name="pageIndex">Zero-based current page index
        /// (0 = first page)</param>
        /// <param name="pageSize">The actual size of each
        /// page</param>
        /// <returns>
        /// A object containing the paged result
        /// and all the relevant paging navigation info.
        /// </returns>
        public static async Task<ApiResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, string sortColumn = null, string sortOrder = null)
        {
            var count = await source.CountAsync();

            if(!string.IsNullOrEmpty(sortColumn) && isValidProperty(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";
                source = source.OrderBy(string.Format("{0} {1}", sortColumn, sortOrder));
            }

            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var data = await source.ToListAsync();

            return new ApiResult<T>(data, count, pageIndex, pageSize, sortColumn, sortOrder);

        }

        /// <summary>
        /// Checks if the given property name exists
        /// to protect against SQL injection attacks
        /// </summary>
        /// <param name="sortColumn"></param>
        /// <returns></returns>
        private static bool isValidProperty(string propertyName, bool throwExcepcionIfNotFound = true)
        {
            var t = typeof(T).GetType();
            var prop = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null && throwExcepcionIfNotFound)
            {
                throw new NotSupportedException(String.Format("ERROR: Property '{0}' does not exists", propertyName));
            }

            return prop != null;
        }
        #endregion Methods
        public List<T> Data { get; private set; }

        public int TotalCount { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }


        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextpage => (PageIndex + 1) < TotalPages;

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }
    }
}
