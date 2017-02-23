using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pupu : MonoBehaviour {
    public GameObject pupuObj;
    public Vector3 upVector;
    public float xVector;
    public Vector3 tempVector;
    public Quaternion tempQuaternion;
    public int isCannon;

    private float nTime;
    private float bTime;
    private GameObject copyObj;

    // Use this for initialization
    void Start () {
        nTime = Time.time;
        bTime = nTime;
	}
	
	// Update is called once per frame
	void Update () {
        nTime = Time.time;
        if (nTime - bTime >= 0.5)
        {
            bTime = nTime;

            isCannon = Random.RandomRange(1, 4);

            Debug.Log(">>Sung thu " + isCannon + " ban<<");

            if (isCannon == 1)
            {
                xVector = 0.13f;
                upVector.z = -220;
                upVector.x = Random.Range(-60f, 60f);
                upVector.y = Random.Range(420f, 480f);
            } 
            else if (isCannon == 2)
            {
                xVector = -5.1f;
                upVector.z = -220;
                upVector.x = Random.Range(100f, 160f);
                upVector.y = Random.Range(450f, 480f);
            }
            else if (isCannon == 3)
            {
                xVector = 4.68f;
                upVector.z = -220;
                upVector.x = Random.Range(-160f, -100f);
                upVector.y = Random.Range(450f, 480f);
            }

            //xVector = Random.RandomRange(-1.5f, 1.5f);
            tempVector.Set(xVector, -2, 0);
            tempQuaternion.Set(0, 0, 0, 0);
            copyObj = Instantiate(pupuObj, tempVector, tempQuaternion);
            copyObj.GetComponent<Rigidbody>().isKinematic = false;
            
            copyObj.GetComponent<Rigidbody>().AddForce(upVector);
            copyObj.GetComponent<Rigidbody>().AddTorque(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
        }
	}
}
