using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

public class LevelActorSpawner : ILevelActorSpawner
{
    private ILayoutGenerator _generator;
    private List<IEnemy> _enemiesCollection;
    private PlayerController _playerController;
    bool[,] Layout;
    int _playerSpawnPosition = 2;
    int _playerHeight = 3;
    List<IEnemy> spawnedEnemies;
    [Inject]
    public LevelActorSpawner(ILayoutGenerator generator)
    {
        _generator = generator;
    }
    [Inject]
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }
    private Vector2 FindPointInColumn(int column, int maxSeekDist)
    {
        Vector2 SpawnPoint = Vector2.negativeInfinity;
        for (int i = 0; i < Layout.GetLength(1); i++) //find enough space starting from the bottom
        {
            if (Layout[column, i] == false)
            {
                bool isObstructed = false;
                for (int seekIndex = 0; seekIndex < maxSeekDist; seekIndex++)
                    if (Layout[column, i + seekIndex] == true)
                    {
                        isObstructed = true;
                        seekIndex = maxSeekDist;
                    }
                if (!isObstructed)
                {
                    SpawnPoint = new Vector2((float)column, (float)i);
                    i = Layout.GetLength(1);
                }
            }
        }
        return SpawnPoint;
    }
    public List<IEnemy> SpawnActors()
    {
        Layout = _generator.Layout;
        //spawn player
        Vector2 PlayerSpawnPoint = FindPointInColumn(_playerSpawnPosition, _playerHeight + 1);
        if (!PlayerSpawnPoint.Equals(Vector2.negativeInfinity))
        {
            PlayerSpawnPoint += new Vector2(0.5f, (float)_playerHeight / 2f + 0.5f); // align model pivot/center of cell
            _playerController.gameObject.SetActive(true);
            _playerController.transform.position = PlayerSpawnPoint;
        }
        else throw new System.Exception("Level generated badly, cannot find place to spawn player");

        //spawn enemies
        List<EnemyPopulation> enemySpawnCandidates = _generator.Config.populations;
        enemySpawnCandidates = enemySpawnCandidates.OrderByDescending(x => x.enemyType.patrolRadius + x.enemyType.playerSeekRadius).ToList();
        List<int> possibleColumns = Enumerable.Range(_playerSpawnPosition, Layout.GetLength(0) - 2).ToList();
        spawnedEnemies = new List<IEnemy>();
        foreach (EnemyPopulation pop in enemySpawnCandidates)
            for (int i = 0; i < pop.quantity; i++)
            {
                int minColumnValue = (int)Mathf.Ceil(pop.enemyType.patrolRadius+pop.enemyType.playerSeekRadius)+ _playerSpawnPosition + 2;
                int minIndex = possibleColumns.FindIndex(c => c >= minColumnValue);
                int columnToSpawnID = Random.Range(minIndex, possibleColumns.Count);
                int columnToSpawn = possibleColumns[columnToSpawnID];
                Vector2 EnemySpawnPoint = FindPointInColumn(columnToSpawn, Mathf.CeilToInt(pop.enemyType.height) + 1);
                if (IsValidPosition(EnemySpawnPoint))
                {
                    EnemySpawnPoint += new Vector2(0.5f, pop.enemyType.height / 2f); // align model pivot/center of cell
                    GameObject enemyGO = GameObject.Instantiate(pop.enemyType.pref, EnemySpawnPoint, Quaternion.identity);
                    IEnemy enemy = enemyGO.GetComponent<IEnemy>();                   
                    enemy.Init(pop.enemyType, _playerController);
                    spawnedEnemies.Add(enemy);
                    possibleColumns.RemoveAll(col => Mathf.Abs(col - columnToSpawn) <= 1);
                }
            }
        return (spawnedEnemies);
    }
    public void DeSpawnActors()
    {
        _playerController.DespawnAllBullets();
        _playerController.gameObject.SetActive(false);
        foreach (IEnemy enemy in spawnedEnemies)
            enemy.Despawn();
    }
    public GameObject GetPlayer()
    {
        return _playerController.gameObject;
    }
    bool IsValidPosition(Vector2 pos)
    {
        return !(float.IsInfinity(pos.x) || float.IsInfinity(pos.y) || float.IsNegativeInfinity(pos.x) || float.IsNegativeInfinity(pos.y) || float.IsNaN(pos.x) || float.IsNaN(pos.y));
    }
}
