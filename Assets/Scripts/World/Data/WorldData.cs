using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "Data/New World Data", order = 1)]
public class WorldData : ScriptableObject
{
    public Vector2 GridSize;
    public float CellSize;
    public float ExtraHeight;

    [Header("Collectables Variable")]
    public int numCollectables;
}
