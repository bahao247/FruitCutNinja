using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    // Use this for initialization
    public Transform track;
    private float moveSpeed = 3;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float move = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, track.position, move);
	}
}
