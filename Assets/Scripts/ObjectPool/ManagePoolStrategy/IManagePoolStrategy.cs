public interface IManagePoolStrategy<T>
{
    public T Get();
    public bool TryGet(out T result);
    public void Return(T obj);
}