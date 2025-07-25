using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class LevelText: MonoBehaviour
{
    private TMP_Text _TextComponent;
    [Inject] GameManager _gameManager;

    void Start()
    {
        _TextComponent = this.GetComponent<TMP_Text>();
        _gameManager.OnLevelChanged += OnLevelChanged;
        // Force init value
        Debug.Log("LevelText Start() called on " + gameObject.name);
        OnLevelChanged(_gameManager.CurrentLevel);
    }
    void OnLevelChanged(int levelID)
    {
        Debug.Log($"{gameObject.name} received level update: {levelID}");
        _TextComponent.text = "Level " + (levelID + 1).ToString();
    }
}
