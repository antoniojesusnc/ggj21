using UnityEngine;

public class UIGame : MonoBehaviour
{
    public static UIGame Instance;
    
    [SerializeField]
    private GameObject _victoryPopup;
    
    [SerializeField]
    private GameObject _gameOverPopup;
    
    [SerializeField]
    private UIDetectionBar _uiDetectionBar;
    
    void Start()
    {
        Instance = this;
    }
    
    public void OpenVictoryPopup()
    {
        _victoryPopup.gameObject.SetActive(true);        
    }
    
    public void OpenGameOver()
    {
        _gameOverPopup.gameObject.SetActive(true);  
    }

    public void SetBarValue(float value)
    {
        _uiDetectionBar.SetBarValue(value);
    }
}
