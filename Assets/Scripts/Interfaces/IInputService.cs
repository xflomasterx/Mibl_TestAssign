using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using System;

[Serializable]
public enum CharacterAction
{
    MoveLeft,
    MoveRight,
    Jump,
    Shoot
}
public class GamePlayButton
{
    public CharacterAction actionID;
    public bool state;
}

public interface IInputService
{
    bool IsActionRequested(CharacterAction actionID);
    void SetActionRequest(CharacterAction actionID, bool state);
    void ReleaseAll();
}