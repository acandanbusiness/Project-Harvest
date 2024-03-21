using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLoader : MonoBehaviour
{
    public Button myButton;
    public string targetSceneName = "Main Menu";

    void Start()
    {

        myButton.onClick.AddListener(LoadTargetScene);
    }

    void LoadTargetScene()
    {

        SceneManager.LoadScene(targetSceneName);
    }
}