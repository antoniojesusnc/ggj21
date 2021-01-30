using UnityEngine;

public class UIGame : MonoBehaviour
{
    public static UIGame Instance;
    
    [SerializeField]
    private GameObject _victoryPopup;
    
    [SerializeField]
    private GameObject _gameOverPopup;

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
    
}
