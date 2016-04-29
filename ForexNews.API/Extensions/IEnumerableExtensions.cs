using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForexNews.API.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Use<T>(this T obj) where T : IDisposable
        {
            try
            {
                yield return obj;
            }
            finally
            {
                obj?.Dispose();
            }
        }
    }
}