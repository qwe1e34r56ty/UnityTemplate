using System;
using System.Collections.Generic;
using System.Linq;

public abstract class AEntityAction
{
    public string id;
    public int priority;
    public List<Action<Entity>> postExecutionCallbacks = new();
    public AEntityAction(string id, int priority)
    {
        this.id = id;
        this.priority = priority;
    }
    public abstract void Attach(GameContext gameContext, Entity entity);
    public abstract void Detach(GameContext gameContext, Entity entity);
    public abstract bool CanExecute(GameContext gameContext, Entity entity);
    public abstract void Execute(GameContext gameContext, Entity entity);
}