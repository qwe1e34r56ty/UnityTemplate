using UnityEngine;

public class TickDispatcher
{
    public void Dispatch(GameContext gameContext)
    {
        foreach (IUpdatable updatable in gameContext.updateHandlers)
        {
            updatable.Update(Time.deltaTime);
        }
    }
}