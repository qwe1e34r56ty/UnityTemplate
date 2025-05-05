public interface IResourceLoaderStrategy<T>
{
    public T Load(string path, int pixelPerUnit = 100);
}