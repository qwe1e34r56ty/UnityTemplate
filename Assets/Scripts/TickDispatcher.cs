using UnityEngine;

public class TickDispatcher
{
    public void Dispatch(GameContext gameContext)
    {
        foreach (IUpdateable updatable in gameContext.updateHandlers)
        {
            updatable.Update(gameContext, Time.deltaTime);
        }
    }
}