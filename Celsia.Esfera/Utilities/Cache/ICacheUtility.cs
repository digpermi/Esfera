using System;

namespace Utilities.Cache
{
    public interface ICacheUtility
    {
        T GetCacheValue<T>(string claveCahce, Func<T> ObtenerValor);
    }
}
