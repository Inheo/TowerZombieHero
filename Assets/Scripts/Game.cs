using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private SpawnerEnemy _spawnerEnemy;

    private int _countKilledEnemy;

    public static Game Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < TemporaryParametrs.CountEnemyPerLevel; i++)
        {
            _spawnerEnemy.SpawnedEnemy[i].OnDead += EnemyKilled;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EnemyKilled()
    {
        _countKilledEnemy++;
        if(_countKilledEnemy == TemporaryParametrs.CountEnemyPerLevel)
        {
            Win();
        }
    }
    private void Win()
    {
        TemporaryParametrs.CurrentLevel++;
        if (TemporaryParametrs.CurrentLevel > TemporaryParametrs.MaxLevel)
        {
            TemporaryParametrs.CurrentLevel = 0;
        }

        StartCoroutine(Delay(2f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name)));
    }

    private IEnumerator Delay(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < TemporaryParametrs.CountEnemyPerLevel; i++)
        {
            _spawnerEnemy.SpawnedEnemy[i].OnDead -= EnemyKilled;
        }
    }
}

public enum LayersName
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    StayPoint = 8,
    Bullet = 10,
    DeadEnemy = 12,
    LivingEnemy = 13,
    Player = 14
}