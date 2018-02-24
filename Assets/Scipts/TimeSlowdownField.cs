using UnityEngine;

public class TimeSlowdownField : MonoBehaviour {

    private TimeManager timeManager;
    private Ship ship;

	void Start () {
		timeManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<TimeManager>();
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Terrain")) {

            if (ship.isActiveAndEnabled && ship.CurrentEnergyLevel() > 0) {
                timeManager.SlowDownTime(true);
            }

        }

    }

    private void OnTriggerExit2D(Collider2D other) {

        if (other.CompareTag("Terrain")) {

            if (timeManager.IsTimeSlowed()) {
                timeManager.SlowDownTime(false);
            }

        }

    }

}
