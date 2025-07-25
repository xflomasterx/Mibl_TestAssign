using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class OverlayInputService : IInputService
{
    private List<GamePlayButton> _inputButtons;
    public OverlayInputService()
    {
        _inputButtons = new List<GamePlayButton>();
        foreach (CharacterAction charAction in Enum.GetValues(typeof(CharacterAction)))
            _inputButtons.Add(new GamePlayButton { actionID = charAction, state = false });
    }
    public bool IsActionRequested(CharacterAction actionID)
    {
        GamePlayButton button = _inputButtons.FirstOrDefault(b => b.actionID == actionID);
        if (button == null)
            return false;
        return button.state;
    }
    public void SetActionRequest(CharacterAction actionID, bool state)
    {
        GamePlayButton button = _inputButtons.FirstOrDefault(b => b.actionID == actionID);
        if (button != null)
            button.state = state;
    }
    public void ReleaseAll()
    {
        foreach (GamePlayButton button in _inputButtons)
            button.state = false;
    }
}
