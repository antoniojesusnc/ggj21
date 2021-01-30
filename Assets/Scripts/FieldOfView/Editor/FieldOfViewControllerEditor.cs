using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfViewController))]
public class FieldOfViewControllerEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfViewController fow = (FieldOfViewController) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.FieldOfViewData.distanceToShow);
        Vector3 viewAngleA = fow.DirectionFromAngle(-fow.FieldOfViewData.angleToShow / 2, false);
        Vector3 viewAngleB = fow.DirectionFromAngle(fow.FieldOfViewData.angleToShow / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.FieldOfViewData.distanceToShow);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.FieldOfViewData.distanceToShow);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.Targets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}