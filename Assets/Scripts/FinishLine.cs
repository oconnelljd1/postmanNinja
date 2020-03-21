using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour {

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject player;
    private float initDistance;

    private void Awake()
    {
       
    }

    // Use this for initialization
    void Start ()
    {
        initDistance = transform.position.z - player.transform.position.z;
	}
	
	// Update is called once per frame
	void Update ()
    {
        slider.value = 1 - ((transform.position.z - player.transform.position.z) / initDistance);
        if (player.transform.position.z > transform.position.z)
            LevelManager.instance.Win();
    }
}
