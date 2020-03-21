using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    private PlayerController _player;
    public static LevelManager instance;

    [SerializeField]
    private Animator _countDown, _levelEnd;

    public void Awake()
    {
        if (instance)
            Object.Destroy(gameObject);
        else
            instance = this;
    }

	// Use this for initialization
	void Start ()
    {
        _countDown.SetTrigger("start");
	}

    public void OnStart()
    {
        _player.OnStart();
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void Lose()
    {
        _player.OnStop();
        _levelEnd.SetTrigger("lose");
    }

    public void Win()
    {
        _levelEnd.SetTrigger("win");
    }
}
