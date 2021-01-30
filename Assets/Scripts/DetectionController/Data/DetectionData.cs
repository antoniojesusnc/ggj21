using UnityEngine;

[CreateAssetMenu(fileName = "DetectionData", menuName = "Data/New Detection Data", order = 1)]
public class DetectionData : ScriptableObject
{
    public float increasePerDetection;
    public float decreasePerSeconds;
    public float maxValue;
}
