using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupuHit : MonoBehaviour {
    private float check;
    private Component[] child;

	// Use this for initialization
	void Start () {
        child = gameObject.GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.position.y <= -3)
        {
            Destroy(gameObject);
        }
        if (child[1].transform.position.y <= -3)
        {
            Destroy(gameObject);
        }
        if (Input.GetMouseButton(0))
        {
            check = 1;
        }
        if (Input.GetMouseButtonUp(0))
        {
            check = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (check == 1)
        {
            if (other.tag == "ball")
            {
                Debug.Log("other.tag == ball");
                gameObject.GetComponent<Rigidbody> ().isKinematic = true;
                gameObject.GetComponent<Collider> ().isTrigger = true;

                child[1].GetComponent<Collider> ().isTrigger = false;
                child[2].GetComponent<Collider> ().isTrigger = false;
                child[1].GetComponent<Rigidbody> ().isKinematic = false;
                child[2].GetComponent<Rigidbody> ().isKinematic = false;

                child[1].GetComponent<Rigidbody> ().AddRelativeForce(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-20, 0));
                child[2].GetComponent<Rigidbody> ().AddRelativeForce(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(0, 20));

            }
        }
    }
}
