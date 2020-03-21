using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	// Use this for initialization
	public void OnStart () {
        LevelManager.instance.OnStart();
	}

    public void RestartLevel()
    {
        SceneController.instance.ReloadScene();
    }

    public void NextLevel()
    {
        SceneController.instance.LoadNextScene();
    }

}
