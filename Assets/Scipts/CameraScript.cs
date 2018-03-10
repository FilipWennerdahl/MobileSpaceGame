using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float verticalOffset;

    private GameObject ship;
    private Vector3 newShipPosition;
    private bool movingTowardsNewShipLocation = false;
    private float moveTowardsSpeed = 15f;
    private RespawnManager respawnManager;
    private bool shipReachedNewLocation = true;

	void Start () {

        respawnManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<RespawnManager>();
        ship = GameObject.FindGameObjectWithTag("Ship");

    }
	
	void LateUpdate () {

        if (movingTowardsNewShipLocation) {
            MoveTowardsShip();
        } else if(!shipReachedNewLocation){
            CheckShipPositionDuringRespawn();
        } else {
            FollowShipVertically();
        }

    }

    public bool ShipAtCorrectYPosition() {
        return shipReachedNewLocation;
    }

    public void StartMovingTowardsShip(Vector3 newPosition) {
        newShipPosition = newPosition;
        newShipPosition.y += verticalOffset;
        newShipPosition.z = transform.position.z;
        movingTowardsNewShipLocation = true;
    }

    private void CheckShipPositionDuringRespawn() {

        if (ship.transform.position.y + verticalOffset > newShipPosition.y) {
            shipReachedNewLocation = true;
        }

    }

    private void MoveTowardsShip() {
        
        if(!ship.activeSelf && Vector2.Distance(transform.position, newShipPosition) > 0) {
            transform.position = Vector3.MoveTowards(transform.position, newShipPosition, moveTowardsSpeed * Time.unscaledDeltaTime); 
        } else {
            movingTowardsNewShipLocation = false;
            shipReachedNewLocation = false;
            respawnManager.RespawnShip();
        }

    }

    private void FollowShipVertically() {
        Vector3 newPosition = transform.position;

        newPosition.y = ship.transform.position.y + verticalOffset;
        transform.position = newPosition;
    }

}
