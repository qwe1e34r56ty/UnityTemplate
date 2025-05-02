public interface IResourceLoaderStrategy<T>
{
    T Load(string path);
}