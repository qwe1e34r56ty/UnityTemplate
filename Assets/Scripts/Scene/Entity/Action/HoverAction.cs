using System.Diagnostics;
using UnityEngine;

public class HoverAction: IAction
{
    public HoverAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        gameContext.onHoverEnterHandlers.Add(entity.root, () =>
        {
            if (gameContext.animationPlayerMap.TryGetValue(entity.root, out AnimationPlayer animationPlayer)){
                animationPlayer.Play(entity.root, gameContext.animationDataMap[entity.GetStat<string>(StatID.HoverEnterAnimationID)]);
            }
        });
        gameContext.onHoverExitHandlers.Add(entity.root, () =>
        {
            if (gameContext.animationPlayerMap.TryGetValue(entity.root, out AnimationPlayer animationPlayer))
            {
                animationPlayer.Play(entity.root, gameContext.animationDataMap[entity.GetStat<string>(StatID.HoverExitAnimationID)]);
            }
        });
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        gameContext.onHoverEnterHandlers.Remove(entity.root);
        gameContext.onHoverExitHandlers.Remove(entity.root);
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
    }
}