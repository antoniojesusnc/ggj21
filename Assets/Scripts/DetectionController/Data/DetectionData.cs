using UnityEngine;

[CreateAssetMenu(fileName = "DetectionData", menuName = "Data/New Detection Data", order = 1)]
public class DetectionData : ScriptableObject
{
    public float increasePerDistance;
    public float decreasePerSeconds;
    public float maxValue;
}
