using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public static SceneController instance;

    void Awake()
    {
        if (instance)
            Object.Destroy(gameObject);
        else
        {
            Object.DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    // Use this for initialization
    public void LoadScene (string scene)
    {
        SceneManager.LoadScene(scene);
	}

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit ()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
    }
}
