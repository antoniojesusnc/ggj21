using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfViewController))]
public class FieldOfViewControllerEditor : Editor
{
    private FieldOfViewController _fow;
    void OnSceneGUI()
    {
        _fow = (FieldOfViewController) target;

        DrawFOV(_fow.FieldOfViewData.coneDistanceToShow,
            _fow.FieldOfViewData.coneAngleToShow,
            Color.white);
        
        DrawFOV(_fow.FieldOfViewData.circleDistanceToShow,
            _fow.FieldOfViewData.circleAngleToShow,
            Color.white);
        
        DrawFOV(_fow.FieldOfViewData.coneDistanceToDetect,
            _fow.FieldOfViewData.coneAngleToDetect,
            Color.yellow);
        
        DrawFOV(_fow.FieldOfViewData.circleDistanceToDetect,
            _fow.FieldOfViewData.circleAngleToDetect,
            Color.yellow);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in _fow.Targets)
        {
            Handles.DrawLine(_fow.transform.position, visibleTarget.position);
        }
    }

    private void DrawFOV(float distance, float angle, Color newColor)
    {
        Color beforeColor = Handles.color;
        Handles.color = newColor;
        Handles.DrawWireArc(_fow.transform.position, Vector3.up, Vector3.forward, 360, distance);
        Vector3 viewAngleA = _fow.DirectionFromAngle(-angle / 2, false);
        Vector3 viewAngleB = _fow.DirectionFromAngle(angle / 2, false);

        Handles.DrawLine(_fow.transform.position, _fow.transform.position + viewAngleA * distance);
        Handles.DrawLine(_fow.transform.position, _fow.transform.position + viewAngleB * distance);
        
        Handles.color = beforeColor;
    }
}