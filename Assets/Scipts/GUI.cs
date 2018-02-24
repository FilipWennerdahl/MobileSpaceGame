using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

    private Ship ship;
    private Slider energyBar;
    private Text currentSpeed;
    private Text currentRange;
    
	void Start () {
        ship = GameObject.Find("Ship").GetComponent<Ship>();
        energyBar = gameObject.GetComponentInChildren<Slider>();
        currentSpeed = GameObject.FindGameObjectWithTag("SpeedGUI").GetComponent<Text>();
        currentRange = GameObject.FindGameObjectWithTag("RangeGUI").GetComponent<Text>();
    }

    void Update () {
        UpdateEnergyBar();
        UpdateCurrentSpeed();
        UpdateRangeCounter();
    }

    private void UpdateEnergyBar() {
        energyBar.value = ship.CurrentEnergyLevel();
    }

    private void UpdateCurrentSpeed() {
        currentSpeed.text = "Speed " + ship.GetCurrentSpeedPercentage() + "%";
    }

    private void UpdateRangeCounter() {
        currentRange.text = "Range " + (int)ship.GetCurrentPosition();
    }

}
