using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum GameState
{
    Initial,
    Playing,
    Completed,
    Pause,
    GameOver,
    MainMenu
}

public class GameManager: MonoBehaviour
{
    public event System.Action<int> OnLevelChanged;
    private ILayoutGenerator _generator;
    private ILevelRenderer _renderer;
    private ILevelActorSpawner _levelActorSpawner;
    private ILevelCompletionController _levelCompletionController;
    private UIManager _uIManager;
    private int _currentLevel = 0;
    private GameState _currentState;

    public int CurrentLevel { get => _currentLevel; }
    void Start()
    {
        _currentState = GameState.Initial;
    }
    [Inject]
    public void Construct(ILayoutGenerator generator, ILevelRenderer renderer, ILevelActorSpawner levelActorSpawner, UIManager uIManager, ILevelCompletionController levelCompletionController)
    {
        _generator = generator;
        _generator.ChangeLevel(_currentLevel);
        _renderer = renderer;
        _uIManager = uIManager;
        _levelActorSpawner = levelActorSpawner;
        _levelCompletionController = levelCompletionController;
        SetState(GameState.MainMenu);
    }
    public void SetState(GameState newState)
    {
        _currentState = newState;
        _uIManager.SwitchUiElementsStates(newState);
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;
        if (newState == GameState.Completed)
            NewLevel();
    }
    public void NewLevel()
    {
        _currentLevel++;
        OnLevelChanged?.Invoke(_currentLevel);
        _generator.ChangeLevel(_currentLevel);
    }
    public void OnPlay()
    {
        SetState(GameState.Playing);
        _renderer.Render(_generator.Generate());
        _levelCompletionController.RegisterEnemies(_levelActorSpawner.SpawnActors(), this);
    }
    public void OnRePlay()
    {
        SetState(GameState.Playing);
        _levelActorSpawner.DeSpawnActors();
        _levelCompletionController.RegisterEnemies(_levelActorSpawner.SpawnActors(), this);
    }
    public void OnHome()
    {
        SetState(GameState.MainMenu);
        _levelActorSpawner.DeSpawnActors();
        _renderer.ClearRender();
        _generator.Deconstruct();
    }
    public void OnNext()
    {
        _levelActorSpawner.DeSpawnActors();
        _renderer.ClearRender();
        _generator.Deconstruct();
        OnPlay();
    }
    public void OnPause()
    {
        SetState(GameState.Pause);
    }
    public void OnUnpause()
    {
        SetState(GameState.Playing);
    }
}