using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    event System.Action OnPlayerTouched;
    void Init(EnemyType enemyType, PlayerController playerController);
    void Despawn();
}
