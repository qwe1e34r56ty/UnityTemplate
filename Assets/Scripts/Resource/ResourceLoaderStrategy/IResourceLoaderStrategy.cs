public interface IResourceLoaderStrategy<T>
{
    public T Load(string path);
}