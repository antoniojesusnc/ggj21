using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDetectionBar : MonoBehaviour
{
    [SerializeField] private DetectionData _detectionData;
    [SerializeField]
    private Image _filledBar;
    [SerializeField]
    private RectTransform _eyeTransform;

    [SerializeField] public float _eyeHeight;

    public void SetBarValue(float value)
    {
        float ratio = value / _detectionData.maxValueThirdState;
        _filledBar.fillAmount = ratio;
        _eyeTransform.sizeDelta = new Vector2(_eyeTransform.sizeDelta.x, _eyeHeight * ratio);
    }
}
