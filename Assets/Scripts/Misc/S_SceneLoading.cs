using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneLoading : MonoBehaviour
{
    [SerializeField] private string presetSceneName = "UItestingMainMenu";
    
    public void LoadNewScene(string sceneName = "")
    {
        SceneManager.LoadScene(sceneName == "" ? presetSceneName : sceneName);
    }
}
