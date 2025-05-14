using UnityEngine;

public interface ISceneCommand
{
    public void Execute(GameContext gameContext, 
        SceneDirector builder);
}