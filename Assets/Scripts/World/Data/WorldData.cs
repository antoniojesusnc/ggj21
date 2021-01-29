using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "Data/New World Data", order = 1)]
public class WorldData : ScriptableObject
{
    public Vector2 GridSize;
    public float CellSize;
}
