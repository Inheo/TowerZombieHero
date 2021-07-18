using UnityEngine;

[CreateAssetMenu(menuName = "SpawnerConfig")]
public class SpawnerEnemyConfig : ScriptableObject
{
    public Level[] Levels;
}

[System.Serializable]
public struct Level
{
    public Vector3[] PositionEnemies;
}
