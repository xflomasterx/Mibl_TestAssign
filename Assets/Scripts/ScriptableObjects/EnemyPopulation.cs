using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Configs/EnemyPopulation")]
public class EnemyPopulation : ScriptableObject
{
    public EnemyType enemyType;
    public int quantity=1;
}
