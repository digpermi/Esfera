namespace Bussines.Adapters
{
    internal interface IAdapter<TEntity, ServiceTEntity> where TEntity : class where ServiceTEntity : class
    {
        TEntity Adapt(ServiceTEntity serviceEntity);
    }
}
