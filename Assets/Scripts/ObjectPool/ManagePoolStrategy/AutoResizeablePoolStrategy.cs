using System.Collections.Generic;

public abstract class AutoResizablePoolStrategy<T> : IManagePoolStrategy<T>, IUpdateable
{
    protected Queue<T> availableNow = new();
    protected Queue<T> returnedLastFrame = new();
    public int capacity { get; protected set; }
    public int inUseCount { get; protected set; } = 0;

    protected AutoResizablePoolStrategy(GameContext gameContext, int initialCapacity)
    {
        gameContext.updateHandlers.Add(this);
        capacity = initialCapacity;
        for (int i = 0; i < capacity; i++)
        {
            availableNow.Enqueue(CreateInstance());
        }
    }

    public bool TryGet(out T ret)
    {
        ret = Get();
        return true;
    }

    public T Get()
    {
        if (availableNow.Count == 0)
        {
            Resize(capacity * 2);
        }

        var obj = availableNow.Dequeue();
        inUseCount++;
        OnGet(obj);
        return obj;
    }

    public void Return(T obj)
    {
        OnReturn(obj);
        returnedLastFrame.Enqueue(obj);
        inUseCount--;
    }

    private void Resize(int newCapacity)
    {
        if (newCapacity < inUseCount)
            return;

        int diff = newCapacity - capacity;

        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                availableNow.Enqueue(CreateInstance());
            }
        }
        else if (diff < 0)
        {
            int toRemove = -diff;
            while (toRemove > 0 && availableNow.Count > 0)
            {
                var obj = availableNow.Dequeue();
                OnDequeue(obj);
                toRemove--;
            }
        }

        capacity = newCapacity;
    }

    public void Update(GameContext gameContext, float deltaTime)
    {
        while (returnedLastFrame.Count > 0)
        {
            availableNow.Enqueue(returnedLastFrame.Dequeue());
        }

        int total = availableNow.Count + inUseCount;

        if (inUseCount < capacity / 4 && capacity > 4 && total < capacity / 2)
        {
            Resize(capacity / 2);
        }

        OnUpdate();
    }

    protected abstract T CreateInstance();
    protected virtual void OnGet(T obj)
    {

    }
    protected virtual void OnReturn(T obj)
    {

    }
    protected virtual void OnDequeue(T obj)
    {

    }

    protected virtual void OnUpdate()
    {

    }
}
