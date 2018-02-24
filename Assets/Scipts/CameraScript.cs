using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float verticalOffset;

    private GameObject ship;
    private Vector3 newShipPosition;
    private bool movingTowardsShip = false;
    private float moveTowardsSpeed = 15f;
    private RespawnManager respawnManager;

	void Start () {

        respawnManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<RespawnManager>();
        ship = GameObject.FindGameObjectWithTag("Ship");

    }
	
	void LateUpdate () {

        if (movingTowardsShip) {
            MoveTowardsShip();
        } else {
            FollowShipVertically();
        }

    }

    public void StartMovingTowardsShip(Vector3 newPosition) {
        newShipPosition = newPosition;
        newShipPosition.y += verticalOffset;
        newShipPosition.z = transform.position.z;
        movingTowardsShip = true;
    }

    public float UnitToPixels(float units) {
        return (Camera.main.pixelHeight / (2 * Camera.main.orthographicSize)) * units;
    }

    private void MoveTowardsShip() {
        
        if(!ship.activeSelf && Vector2.Distance(transform.position, newShipPosition) > 0) {
            transform.position = Vector3.MoveTowards(transform.position, newShipPosition, moveTowardsSpeed * Time.unscaledDeltaTime); 
        } else {
            movingTowardsShip = false;

            respawnManager.RespawnShip();
        }

    }

    private void FollowShipVertically() {
        Vector3 newPosition = transform.position;

        newPosition.y = ship.transform.position.y + verticalOffset;
        transform.position = newPosition;
    }

}
