using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoadSceneWhenClickKey : MonoBehaviour
{
    [SerializeField] private List<KeyCode> _keys;
    [SerializeField] private int _sceneIndex;
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (Input.GetKeyDown(_keys[i]))
            {
                SceneManager.LoadScene(_sceneIndex);
            }
        }
    }
}
