namespace dotnetliberty.DependencyInjection
{
    public interface IServiceFactory<T> where T : class
    {
        T Build();
    }
}