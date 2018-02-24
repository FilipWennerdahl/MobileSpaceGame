using UnityEngine;
using System.Diagnostics;

public class TimeManager : MonoBehaviour {

    public float minimumTimeScale;

    private float lastSlowdownOccasion = 0;
    private float defaultTimeScale = 1f;
    private bool timeSlowedDown = false;
    private float originalFixedDeltaTime;
    private Ship ship;
    private Stopwatch stopswatch;

    void Start() {
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        stopswatch = new Stopwatch();
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update() {
        TimeControl();
    }
    
    public void SlowDownTime(bool slowedDown) {

        if(!slowedDown) {
            stopswatch.Reset();
            stopswatch.Start();
        }

        timeSlowedDown = slowedDown;
    }

    public bool IsTimeSlowed() {
        return timeSlowedDown;
    }

    public float TimeElapsedSinceSlowdownMilliseconds() {
        return stopswatch.ElapsedMilliseconds;
    }

    public void ResetTimeScale() {
        SlowDownTime(false);

        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }

    private void TimeControl() {

        if (timeSlowedDown && Time.timeScale > minimumTimeScale && ship.CurrentEnergyLevel() > 0) {
            Time.timeScale = Mathf.Clamp(Time.timeScale - (1f / SlowdownMultiplier()) * Time.unscaledDeltaTime, minimumTimeScale, defaultTimeScale);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        } else if(Time.timeScale < defaultTimeScale){
            Time.timeScale = Mathf.Clamp(Time.timeScale + (1f / 0.5f) * Time.unscaledDeltaTime, minimumTimeScale, defaultTimeScale);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

    }

    private float SlowdownMultiplier() {
        return Mathf.Clamp((0.15f - ((ship.GetCurrentSpeedPercentage() - 100f) / 100f) * 0.1f), 0.01f, 0.15f);
    }

}
