using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed = 1;
    [SerializeField]
    private GameObject letterPrefab, congratsPrefab;
    [SerializeField]
    private float _jumpHeight = 2.5f, _jumpSpeed = 3, _fallSpeed = 7;
    private Animator animator;

    private enum States { Inactive, Idle, Jumping, Throwing, Sneaking };
    private States state = States.Inactive;

    private List<GameObject> _sneaks = new List<GameObject>();
    private GameObject _currentSneak = null;
    private bool _canSneak = true;

    void Awake()
    {
        animator = GetComponent<Animator>();
        foreach(Transform child in transform.GetChild(2))
        {
            child.gameObject.SetActive(false);
            _sneaks.Add(child.gameObject);
        }
    }

    // Use this for initialization
    public void OnStart ()
    {
        state = States.Idle;
        Debug.Log(animator);
        animator.SetTrigger("idle");
	}

    public void OnStop()
    {
        StopAllCoroutines();
        animator.SetTrigger("die");
        StartCoroutine("deathCoroutine");
        state = States.Inactive;
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(state)
        {
            case States.Inactive:
                // do other things
                return;
            case States.Idle:
                if (Input.GetButtonDown("Jump"))
                {
                    Debug.Log("jumping");
                    StartCoroutine("jumpCoroutine");
                    break;
                }
                if (Input.GetButtonDown("Attack"))
                {
                    StartCoroutine("throwLetterCoroutine");
                    break;
                }
                if(Input.GetButtonDown("Sneak") && _canSneak)
                {
                    Debug.Log("StartSneak");
                    startSneak();
                }
                break;
            case States.Jumping:
                //do other things
                break;
            case States.Throwing:
                break;
            case States.Sneaking:
                if(Input.GetButtonUp("Sneak") && _canSneak)
                {
                    stopSneak();
                }
                break;
        }
        transform.position += (Vector3.forward * _moveSpeed * Time.deltaTime);
        Debug.Log(state);
	}

    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log("playerTriggerEnter");
        if (other.tag == "Obstacle" ||
            (other.tag == "Enemy" && state != States.Sneaking))
        {
            LevelManager.instance.Lose();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Mailbox" && !other.GetComponent<MailboxController>().active) || other.tag == "Enemy")
            congrats();
    }

    private IEnumerator jumpCoroutine()
    {
        state = States.Jumping;
        float startTime = Time.time;
        Vector3 startPos = transform.position;
        Vector3 lastPos = transform.position;
        while(lastPos.y <= transform.position.y)
        {
            lastPos = transform.position;
            Vector3 temp = transform.position;
            temp.y = startPos.y + (_jumpHeight * Mathf.Sin((Time.time - startTime) * _jumpSpeed));
            transform.position = temp;
            yield return null;
        }
        startPos = transform.position;
        startTime = Time.time;
        while(state == States.Jumping)
        {
            Vector3 temp = transform.position;
            temp.y = startPos.y - Mathf.Pow(_jumpHeight, (Time.time - startTime) * _fallSpeed) + 1;
            transform.position = temp;
            
            Collider[] groundTest = Physics.OverlapSphere(transform.position, 0.1f);
            for (int i = 0; i < groundTest.Length; i++)
            {
                state = groundTest[i].tag == "Ground" ? States.Idle : States.Jumping;
                if (state == States.Idle) break;
            }
            yield return null;
        }
        bool trash = false;
        Collider[] trashTest = Physics.OverlapSphere(transform.position + Vector3.back, 0.5f);
        for (int i = 0; i < trashTest.Length; i++)
        {
            trash = trashTest[i].tag == "Obstacle";
            if (trash)
            {
                congrats();
                break;
            }
        }
    }

    private IEnumerator throwLetterCoroutine()
    {
        Debug.Log("Attack ButtonDown");
        state = States.Throwing;
        animator.SetTrigger("throw");

        Vector3 startPos = transform.GetChild(0).position;
        float startTime = Time.time;
        MailboxController targetMailbox = null;
        Collider[] mailboxColliders = Physics.OverlapSphere(transform.position, 0.5f);
        for (int i = 0; i < mailboxColliders.Length; i++)
        {
            targetMailbox = mailboxColliders[i].GetComponent<MailboxController>();
            if (targetMailbox && targetMailbox.active)
            {
                // IEnumerator coroutine= ;
                targetMailbox.StartCoroutine(targetMailbox.RecieveMail(startPos));
                break;
            }
        }
        if (!targetMailbox)
        {
            GameObject letter = Instantiate(letterPrefab, startPos, letterPrefab.transform.rotation) as GameObject;
            Rigidbody letterRB = letter.GetComponent<Rigidbody>();
            letterRB.AddTorque(Vector3.up * 1000);
            letterRB.useGravity = true;
            letterRB.AddForce(Vector3.left * 10);
        }
        while (Time.time - startTime < 0.5f)
        {
            yield return null;
        }
        state = States.Idle;
    }

    private void startSneak()
    {
        state = States.Sneaking;
        transform.GetChild(1).gameObject.SetActive(false);
        _currentSneak = _sneaks[Random.Range(0, _sneaks.Count)].gameObject;
        _currentSneak.SetActive(true);
        StartCoroutine("smokeCoroutine");
    }

    private IEnumerator smokeCoroutine()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        float startTime = Time.time;
        _canSneak = false;
        while(Time.time - startTime < 1)
        {
            yield return null;
        }
        transform.GetChild(3).gameObject.SetActive(false);
        _canSneak = true;
        if(!Input.GetButton("Sneak") && state == States.Sneaking)
        {
            stopSneak();
        }
    }

    private void stopSneak()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        _currentSneak.SetActive(false);
        StopCoroutine("smokeCoroutine");
        StartCoroutine("smokeCoroutine");
        state = States.Idle;
    }

    private IEnumerator deathCoroutine()
    {
        float startTime = Time.time;
        while(Time.time - startTime < 0.333333333f)
        {
            Vector3 temp = transform.GetChild(1).position;
            temp.y -= 0.04f;
            transform.GetChild(1).position = temp;
            yield return null;
        }
    }

    private void congrats()
    {
        GameObject obj = Object.Instantiate(congratsPrefab, Vector3.zero, congratsPrefab.transform.rotation);
        obj.GetComponent<Animator>().SetFloat("random", Random.value);
        obj.transform.parent = transform;
        obj.transform.localPosition = congratsPrefab.transform.position;
        Destroy(obj, 1.0f);
    }

}
