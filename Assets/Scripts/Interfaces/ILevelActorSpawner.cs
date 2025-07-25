using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelActorSpawner
{
    List<IEnemy> SpawnActors();
    void DeSpawnActors();
    GameObject GetPlayer();
}
