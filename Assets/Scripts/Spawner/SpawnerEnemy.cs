using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private SpawnerEnemyConfig _config;

    [SerializeField] private Enemy _prefab;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();

    public List<Enemy> SpawnedEnemy => _spawnedEnemies;

    private void Awake()
    {
        TemporaryParametrs.CountEnemyPerLevel = _config.Levels[TemporaryParametrs.CurrentLevel].PositionEnemies.Length;

        for (int i = 0; i < _config.Levels[TemporaryParametrs.CurrentLevel].PositionEnemies.Length; i++)
        {
            Enemy enemy = Instantiate(_prefab);
            enemy.transform.position = _config.Levels[TemporaryParametrs.CurrentLevel].PositionEnemies[i];
            enemy.transform.rotation = Quaternion.Euler(0, 180, 0);

            _spawnedEnemies.Add(enemy);
        }   
    }
}
