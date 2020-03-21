using UnityEngine;
using System.Collections;

public class MailboxController : MonoBehaviour {

    public bool active { get; private set; }
    private Animator _animator;
    private Rigidbody letter;


	// Use this for initialization
	void Awake ()
    {
        _animator = GetComponent<Animator>();
        active = true;
        letter = transform.GetChild(1).gameObject.GetComponent<Rigidbody>();
        letter.gameObject.SetActive(false);
	}

    public IEnumerator RecieveMail(Vector3 startPos)
    {
        letter.gameObject.SetActive(true);
        active = false;
        _animator.SetTrigger("armUp");
        Vector3 targetPos = transform.GetChild(0).position;
        float startTime = Time.time;
        letter.AddTorque(Vector3.up * 1000);
        while (letter.transform.position != targetPos)
        {
            letter.transform.position = Vector3.Lerp(startPos, targetPos, Time.time - startTime);
            yield return null;
        }
        letter.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(active);
        if(other.tag == "Player" && active)
        {
            LevelManager.instance.Lose();
        }
    }
}
