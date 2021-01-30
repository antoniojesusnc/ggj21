using UnityEngine;
[CreateAssetMenu(fileName = "FieldOfViewData", menuName = "Data/New Field Of View Data", order = 1)]
public class FieldOfViewData : ScriptableObject
{
    [Header("Field of View")]
    [Header("Show Player variables")]
    public float distanceToShow;
    [Range(0,360)]
    public float angleToShow;
    
    [Header("Detect Player variables")]
    public float distanceToDetect;
    [Range(0,360)]
    public float angleToDetect;
    
    [Header("Other variables for better FOV")]
    public float presition;
    public float edgeResolveIterations;
    public float edgeDistTreshold;

    
    [Header("Field of Light")] public float lightExtraAngle;
    public float lightExtraDistance;
}
