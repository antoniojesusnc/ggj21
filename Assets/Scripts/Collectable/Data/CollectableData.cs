using UnityEngine;

[CreateAssetMenu(fileName = "CollectableData", menuName = "Data/New Collectable Data", order = 1)]
public class CollectableData : ScriptableObject
{
    public float floatingDuration;
    public float heightIncrement;
}
