using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour {

    public float distanceToMoveShipBack;
    public int numberOfContinues;

    private Ship ship;
    private LevelRandomizer levelRandomizer;
    private CameraScript cameraScript;
    private TimeManager timeManager;
    private Text continuesGUI;
    private GameObject gameOverSplash;
    private Text finalRangeGUI;

	void Start () {

        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        levelRandomizer = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<LevelRandomizer>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        timeManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<TimeManager>();
        continuesGUI = GameObject.FindGameObjectWithTag("ContinuesGUI").GetComponent<Text>();
        gameOverSplash = GameObject.FindGameObjectWithTag("GameOverSplash");
        finalRangeGUI = GameObject.FindGameObjectWithTag("FinalRangeGUI").GetComponent<Text>();

        gameOverSplash.SetActive(false);
        UpdateContinuesGUI();

    }


    public void RespawnShip() {
        
        if (numberOfContinues < 0) {
            finalRangeGUI.text = "Rached a final range of " + (int)ship.GetCurrentPosition();
            gameOverSplash.SetActive(true);
        } else {
            ship.gameObject.transform.position = new Vector2(ship.gameObject.transform.position.x, Camera.main.transform.position.y - Helper.PixelToUnit(Screen.height) - transform.localScale.y);  
            ship.gameObject.SetActive(true);
        }
        
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    
    public Vector3 GenerateNewShipPosition() {
        Vector3 newShipPosition = levelRandomizer.GetActiveScreenPosition();

        newShipPosition.y -= distanceToMoveShipBack;
        newShipPosition.x = 0;

        return newShipPosition;
    }

    public void DisableShip() {
        numberOfContinues--;

        if (numberOfContinues > -1) {
            UpdateContinuesGUI();
        }

        Vector3 newShipPosition = GenerateNewShipPosition();

        ship.gameObject.SetActive(false);
        ship.NewShipPosition(newShipPosition);
        levelRandomizer.DestroyDeadScreen();
        timeManager.ResetTimeScale();
        cameraScript.StartMovingTowardsShip(newShipPosition);
    }

    private void UpdateContinuesGUI() {
        continuesGUI.text = numberOfContinues + "x";
    }

}
