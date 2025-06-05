using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IdleAnimationAction : IAction
{
    private Dictionary<Entity, AnimationPlayer> animationPlayers = new();
    public IdleAnimationAction()
    {

    }

    public void Attach(GameContext gameContext, Entity entity, int priority)
    {
        if (entity.TryGetStat<string>(StatID.IdleAnimation, out string animationID))
        {
            if (gameContext.animationDataMap.TryGetValue(animationID, out (Sprite[], AnimationPath) animationData))
            {
                AnimationPlayer animationPlayer = new AnimationPlayer();
                animationPlayers.Add(entity, animationPlayer);
                animationPlayer.Play(gameContext, entity.root, animationData);
                gameContext.animationPlayerMap.Add(entity.root, animationPlayer);
            }
        }
    }

    public void Detach(GameContext gameContext, Entity entity)
    {
        if(animationPlayers.TryGetValue(entity, out AnimationPlayer animationPlayer))
        {
            gameContext.animationPlayerMap.Remove(entity.root);
            animationPlayers[entity].Pause(gameContext, entity.root);
            animationPlayers.Remove(entity);
        }
    }

    public bool CanExecute(GameContext gameContext, Entity entity, float deltaTIme)
    {
        return false;
    }

    public void Execute(GameContext gameContext, Entity entity, float deltaTIme)
    {
    }
}