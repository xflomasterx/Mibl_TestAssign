using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelCompletionController : ILevelCompletionController
{
    public List<IEnemy> Enemies { get; set; }
    private  GameManager _gameManager;

    public void RegisterEnemies(List<IEnemy> enemies, GameManager gameManager)
    {
        _gameManager = gameManager;
        Bullet.EnemyHit += OnEnemyHit;
        Enemies = enemies;
        foreach (var enemy in Enemies)
        {
            enemy.OnPlayerTouched += OnPlayerTouched;
        }
    }
    public void OnPlayerTouched()
    {
        _gameManager.SetState(GameState.GameOver);
    }
    public void OnEnemyHit(IEnemy enemy)
    {
        if (Enemies.Remove(enemy))
        {
            enemy.Despawn(); 
            if (Enemies.Count == 0)
            {
                _gameManager.SetState(GameState.Completed);
            }
        }
    }
}
