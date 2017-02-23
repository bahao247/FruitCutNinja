using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePos : MonoBehaviour {
    public GameObject ball;

    private RaycastHit[] hits;
    private RaycastHit hit;
    private Ray ray;
    private float check;
    public Camera camera;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0))
		{
            ball.SetActive(true);
            Pos();
		}
        else
        {
            ball.SetActive(false);
        }
	}

    void Pos()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Logs raycast: " + ray);

        hits = Physics.RaycastAll(camera.transform.position, ray.direction, 50);

        Debug.Log("Logs Hits: " + ray);

        for (int i = 0; i < hits.Length; i++)
        {
            hit = hits[i];
            if (hit.collider.tag == "screen")
            {
                Debug.Log("Logs Hits = screen");
                ball.transform.position = hit.point;
            }
        }
    }
}
