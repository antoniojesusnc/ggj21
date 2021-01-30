using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoadScene : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
