using UnityEngine;

public interface ISceneCommand
{
    void Execute(GameContext gameContext, 
        SceneDirector builder);
}