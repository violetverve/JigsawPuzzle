using UI.LoadScene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

    private void OnEnable()
    {
        LoadBar.LoadCompleted += LoadCompleted;
    }

    private void OnDisable()
    {
        LoadBar.LoadCompleted -= LoadCompleted;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void LoadCompleted()
    {
        SceneManager.LoadScene("UIScene");
    }
}
