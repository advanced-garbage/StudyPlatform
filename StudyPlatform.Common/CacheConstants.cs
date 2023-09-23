using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Common
{
    public static class CacheConstants
    {
        /// <summary>
        /// Cache key for storing every available category.
        /// </summary>
        public const string AllCategoriesCacheKey = "allCategoriesKey";

        /// <summary>
        /// Cache key for invoking the cache for the account profile.
        /// </summary>
        public const string AccountCacheKey = "accountProfileKey";
    }
}
