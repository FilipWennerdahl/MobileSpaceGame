using UnityEngine;

public class ShipBody : MonoBehaviour {

    private RespawnManager respawnManager;

    void Start () {
        respawnManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<RespawnManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.CompareTag("Terrain")) {
            respawnManager.DisableShip();
        }

    }

}
