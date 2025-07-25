using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UIIngame;
    [SerializeField] GameObject UiMenusBlockingUnderlay;
    [SerializeField] GameObject LvlFrame;
    [SerializeField] GameObject MenuMain;
    [SerializeField] GameObject MenuPause;
    [SerializeField] GameObject MenuGameOver;
    [SerializeField] GameObject MenuCompleted;

    public void SwitchUiElementsStates(GameState newState)
    {
        UIIngame.SetActive(newState != GameState.MainMenu);
        UiMenusBlockingUnderlay.SetActive(newState != GameState.Playing);
        LvlFrame.SetActive(newState != GameState.MainMenu);
        MenuMain.SetActive(newState == GameState.MainMenu);
        MenuPause.SetActive(newState == GameState.Pause);
        MenuGameOver.SetActive(newState == GameState.GameOver);
        MenuCompleted.SetActive(newState == GameState.Completed);
    }
}
