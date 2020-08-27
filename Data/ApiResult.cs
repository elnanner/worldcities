using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldCities.Data
{
    public class ApiResult<T>
    {
        /// <summary>
        /// Private constructor calle by te CreateAsync method.
        /// </summary>
        private ApiResult(List<T> data, int count, int pageIndex, int pageSize)
        {
            Data = data;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)(Math.Ceiling(count / (double)pageSize));
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
        public static async Task<ApiResult<T>> CreateAsync(IQueryable<T>source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var data = await source.ToListAsync();

            return new ApiResult<T>(data, count, pageIndex, pageSize);

        }
        #endregion Methods
        public List<T> Data { get; private set; }

        public int TotalCount { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }


        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextpage => (PageIndex + 1) < TotalPages;
    }
}
