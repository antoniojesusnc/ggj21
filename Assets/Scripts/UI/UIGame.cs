using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _victoryPopup;
    
    [SerializeField]
    private GameObject _gameOverPopup;
    
    public void OpenVictoryPopup()
    {
        _victoryPopup.gameObject.SetActive(true);        
    }
    
    public void OpenGameOver()
    {
        _gameOverPopup.gameObject.SetActive(true);  
    }
    
}
