using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputService : IInputService
{
    public bool JumpPressed => Input.GetKey(KeyCode.UpArrow);
    public bool ShootPressed => Input.GetKeyDown(KeyCode.Space);
    public bool LeftPressed => Input.GetKey(KeyCode.LeftArrow);
    public bool RightPressed => Input.GetKey(KeyCode.RightArrow);

    Dictionary<CharacterAction, KeyCode> keybindings;

    public KeyboardInputService()
    {
        //replace if keybindings config added with provided data
        keybindings = new Dictionary<CharacterAction, KeyCode>();
        keybindings.Add(CharacterAction.Jump, KeyCode.UpArrow);
        keybindings.Add(CharacterAction.Shoot, KeyCode.Space);
        keybindings.Add(CharacterAction.MoveLeft, KeyCode.LeftArrow);
        keybindings.Add(CharacterAction.MoveRight, KeyCode.UpArrow);
    }
    public bool IsActionRequested(CharacterAction actionID)
    {
        return Input.GetKey(keybindings[actionID]);
    }
    public void SetActionRequest(CharacterAction actionID, bool state) { }
    public void ReleaseAll() { }
}