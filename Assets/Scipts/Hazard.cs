using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

    [Range(0, 100)]
    public float travelSpeed;

    private float rotationSpeed;
    private Vector2 travelDirection;

	void Start () {
        rotationSpeed = Random.Range(40f, 200f);

        float newXDirection;
        float newYDirection;

        if(transform.localPosition.x > 0) {
            newXDirection = Random.Range(-1f, 0f);
        } else {
            newXDirection = Random.Range(0f, 1f);
        }

        if (transform.localPosition.y > 0) {
            newYDirection = Random.Range(-1f, 0f);
        } else {
            newYDirection = Random.Range(0f, 1f);
        }

        travelDirection = new Vector2(newXDirection * 100, newYDirection * 100).normalized * 100;

    }
	
	void Update () {
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, travelDirection, travelSpeed * Time.deltaTime);
	}
}
