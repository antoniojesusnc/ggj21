using UnityEngine;
[CreateAssetMenu(fileName = "FieldOfViewData", menuName = "Data/New Field Of View Data", order = 1)]
public class FieldOfViewData : ScriptableObject
{
    [Header("Cone Show Player variables")]
    [Header("Field of View CONE")]
    public float coneDistanceToShow;
    [Range(0,360)]
    public float coneAngleToShow;
    
    [Header("Cone Detect Player variables")]
    public float coneDistanceToDetect;
    [Range(0,360)]
    public float coneAngleToDetect;
    
    [Header("Circle around Show Player variables")]
    [Header("Field of View CIRCLE")]
    public float circleDistanceToShow;
    [Range(0,360)]
    public float circleAngleToShow;
    
    [Header("Circle around Detect Player variables")]
    public float circleDistanceToDetect;
    [Range(0,360)]
    public float circleAngleToDetect;
    
    
    [Header("Other variables for better FOV")]
    public float accuracy;
    public float edgeResolveIterations;
    public float edgeDistTreshold;

    
    [Header("Field of Light")] public float lightExtraAngle;
    public float lightExtraDistance;
    
    [Header("Anim switch LookDirection")] public float timeToChangeLookDirection;
}
