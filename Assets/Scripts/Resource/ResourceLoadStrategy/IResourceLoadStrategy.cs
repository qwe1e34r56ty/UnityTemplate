public interface IResourceLoadStrategy<T>
{
    public T Load(string path, int pixelPerUnit = 100);
}