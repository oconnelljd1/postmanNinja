using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenController : MonoBehaviour {

	// Use this for initialization
	public void LoadNextScene()
    {
        SceneController.instance.LoadNextScene();
    }

    public void LoadScene(string scene)
    {
        SceneController.instance.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
