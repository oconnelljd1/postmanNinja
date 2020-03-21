using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed = 1;
    [SerializeField]
    private GameObject _player;
    private bool _started = false;

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(_started)
        {
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
            var temp = transform.position;
            temp.y = _player.transform.position.y;
            transform.position = temp;
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log("EnemyTriggerEnter");
        if(other.tag == "Trigger")
        {
            _started = true;
        }
    }
}
