using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public GameObject[] enemies;
    public int amount;
    private Vector3 spawnPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        enemies = GameObject.FindGameObjectsWithTag("fruit");
        amount = enemies.Length;

        if (amount != 100)
        {
            InvokeRepeating("spawnEmnemy", 1.0f, 3.0f);
        }
    }

    void spawnEmnemy()
    {
        Debug.Log(">>Debug Log: run void spawnEnemy()<<");
        spawnPoint.x = Random.Range(-20, 20);
        spawnPoint.y = 0.3f;
        spawnPoint.z = Random.Range(-20, 20);

        Instantiate(enemies[Random.Range(0, enemies.Length - 1)], spawnPoint, Quaternion.identity);
        CancelInvoke();
    }
}
