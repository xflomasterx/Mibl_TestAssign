using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelCompletionController
{
    List<IEnemy> Enemies { get; set; }
    void RegisterEnemies(List<IEnemy> enemies, GameManager gameManager);
}
