using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button myButton;
    public string targetSceneName = "SampleScene"; 

    void Start()
    {
        
        myButton.onClick.AddListener(LoadTargetScene);
    }

    void LoadTargetScene()
    {
        
        SceneManager.LoadScene(targetSceneName);
    }
}