using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private PlayerController _player;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 temp = transform.position;
        temp.z = _player.transform.position.z + 3;
        transform.position = temp;
	}
    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log("CameraTriggerEnter");
    }
}
