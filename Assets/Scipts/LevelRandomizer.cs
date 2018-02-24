using UnityEngine;

public class LevelRandomizer : MonoBehaviour {

    public GameObject[] slowScreens;
    public GameObject[] mediumScreens;
    public GameObject[] fastScreens;
    public int newScreenOffset;

    private Ship ship;
    private float lastSpawnLocation = 0f;
    private float screenHeight;
    private GameObject activeScreen;
    private GameObject deadScreen;
    private float newScreenPositionMultiplier = 2.5f;

    void Start () {
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        screenHeight = Camera.main.orthographicSize * 2.0f;
        Vector3 newScreenPotition = Vector3.zero;
        newScreenPotition.y = screenHeight;
        activeScreen = Instantiate(slowScreens[RandomizeNewScreen(slowScreens)], newScreenPotition, new Quaternion(0, 0, 0, 0), null);
    }
	
	void Update () {
        PlaceNewScreen();
	}

    public void DestroyDeadScreen() {

        if (deadScreen != null) {
            Destroy(deadScreen);
        }
        
        deadScreen = null;
    }

    public Vector3 GetActiveScreenPosition() {
        return activeScreen.transform.position;
    }

    private void PlaceNewScreen() {

        if (ReadyToSpawnScreen()) {
            GameObject newScreen;

            float currentSpeed = ship.GetCurrentSpeedPercentage();

            if (currentSpeed < 175) {
                newScreen = Instantiate(slowScreens[RandomizeNewScreen(slowScreens)], NewScreenPosition(), new Quaternion(0, 0, 0, 0), null);
            } else if (currentSpeed < 250) {
                newScreen = Instantiate(mediumScreens[RandomizeNewScreen(mediumScreens)], NewScreenPosition(), new Quaternion(0, 0, 0, 0), null);
            } else {
                newScreen = Instantiate(fastScreens[RandomizeNewScreen(fastScreens)], NewScreenPosition(), new Quaternion(0, 0, 0, 0), null);
            }

            DestroyDeadScreen();

            deadScreen = activeScreen;
            activeScreen = newScreen;

        }

    }

    private bool ReadyToSpawnScreen() {
        return ship.transform.position.y > activeScreen.transform.position.y + screenHeight + newScreenOffset;
    }

    private Vector3 NewScreenPosition() {
        Vector3 newScreenPotition = Vector3.zero;

        newScreenPotition.y = activeScreen.transform.position.y + (screenHeight * newScreenPositionMultiplier) + newScreenOffset;

        return newScreenPotition;
    }

    private int RandomizeNewScreen(GameObject[] screens) {
        return Random.Range(0, screens.Length);
    }

}

