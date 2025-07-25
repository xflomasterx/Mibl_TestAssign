using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OverlayInputHandler : MonoBehaviour
{
    IInputService _inputService;

    [Inject]
    public void Init(IInputService inputService)
    {
        _inputService = inputService;
    }
    void OnOverlayButton(CharacterAction requestedAction, bool state)
    {
        _inputService.SetActionRequest(requestedAction, state);
    }
    public void OnOverlayButtonLeft(bool state) => OnOverlayButton(CharacterAction.MoveLeft, state);
    public void OnOverlayButtonRight(bool state) => OnOverlayButton(CharacterAction.MoveRight, state);
    public void OnOverlayButtonJump(bool state) => OnOverlayButton(CharacterAction.Jump, state);
    public void OnOverlayButtonShoot(bool state) => OnOverlayButton(CharacterAction.Shoot, state);
}
