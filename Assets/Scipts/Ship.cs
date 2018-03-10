using UnityEngine;

public class Ship : MonoBehaviour {

    public float verticalStartSpeed;
    public float verticalSpeedIncreaseIncrement;
    public float horizontalSpeed;
    public float energyRegenCooldownMilliseconds;

    private float verticalSpeed;
    private TimeManager timeManager;
    private float originalHorizontalSpeed;
    private float horizontalSlowdownSpeedChangeIncrament = 80;
    private float energy = 100;
    private float energyCostPerDetlaFrame = 100f;
    private float energyRecoveryPerDetlaFrame = 300f;
    private PolygonCollider2D slowDownField;
    private float originalLengthOfSlowndownField;
    private float startPosition;
    private float shipPixelWidth;
    private CameraScript mainCamera;
    private bool reaccelerating = false;
    private float verticalSpeedAccelerationGoal;
    private bool touchAvailable = false;

    void Start () {
        timeManager = GameObject.FindGameObjectWithTag("SystemManager").GetComponent<TimeManager>();
        mainCamera = Camera.main.GetComponent<CameraScript>();
        slowDownField = gameObject.GetComponentInChildren<PolygonCollider2D>();
        originalHorizontalSpeed = horizontalSpeed;
        verticalSpeed = verticalStartSpeed;
        originalLengthOfSlowndownField = slowDownField.points[0].y;
        startPosition = gameObject.transform.position.y;
        shipPixelWidth = Helper.UnitToPixel(gameObject.GetComponent<MeshRenderer>().bounds.size.x);
        if(SystemInfo.deviceType == DeviceType.Handheld) {
            touchAvailable = true;
        }
    }
    void FixedUpdate() {

    }
    void Update () {
        MoveHorizontaly();
        MoveVertically();
        IncreaseSpeed();
        HandleEnergy();
        AdjustSlowdownField();
    }

    public void IncreaseHorizontalSpeed() {
        horizontalSpeed = Mathf.Clamp(horizontalSpeed + (horizontalSlowdownSpeedChangeIncrament * Time.unscaledDeltaTime), originalHorizontalSpeed, MaxHorizontalSpeed());
    }

    public void DefaultHorizontalSpeed() {
        horizontalSpeed = Mathf.Clamp(horizontalSpeed - (horizontalSlowdownSpeedChangeIncrament * Time.unscaledDeltaTime), originalHorizontalSpeed, MaxHorizontalSpeed());
    }

    public int CurrentEnergyLevel() {
        return Mathf.RoundToInt(energy);
    }

    public float GetCurrentSpeedPercentage() {
        return (float)System.Math.Round(verticalSpeed / verticalStartSpeed * 100, 1);
    }

    public float GetCurrentPosition() {
        return gameObject.transform.position.y - startPosition;
    }

    public void NewShipPosition(Vector2 position) {
        gameObject.transform.position = position;
        ResetValues();
    }

    public void Reaccelerate() {
        verticalSpeedAccelerationGoal = verticalSpeed;
        verticalSpeed = verticalStartSpeed;
        reaccelerating = true;
    }

    private void ResetValues() {
        horizontalSpeed = originalHorizontalSpeed;
        //verticalSpeed = verticalSpeed * 0.9f;
        energy = 100f;

        if (verticalSpeed < verticalStartSpeed) {
            verticalSpeed = verticalStartSpeed;
        }

    }

    private float MaxHorizontalSpeed() {
        return 20 + ((GetCurrentSpeedPercentage() - 100f) / 100f) * 8;
    }

    private void HandleEnergy() {

        if (timeManager.IsTimeSlowed()) {
            energy = Mathf.Clamp(energy - (energyCostPerDetlaFrame * Time.unscaledDeltaTime), 0f, 100f);
        } else if(timeManager.TimeElapsedSinceSlowdownMilliseconds() > energyRegenCooldownMilliseconds){
            energy = Mathf.Clamp(energy + (energyRecoveryPerDetlaFrame * Time.unscaledDeltaTime), 0f, 100f);
        }

    }

    private void MoveVertically() {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + transform.localScale.y), verticalSpeed * Time.deltaTime);      
    }

    private void MoveHorizontaly() {

        if(timeManager.IsTimeSlowed()) {
            IncreaseHorizontalSpeed();
        } else {
            DefaultHorizontalSpeed();
        }

        if (mainCamera.ShipAtCorrectYPosition() && Camera.main.WorldToScreenPoint(transform.position).x - (shipPixelWidth / 2) > 0.1) {

            if(touchAvailable) {
                MoveLeftWithTouch();
            } else {
                MoveLeftWithInput();
              
            }

            if(Camera.main.WorldToScreenPoint(transform.position).x - (shipPixelWidth / 2) < 0) {
                float leftBoundaryAligned = -(Helper.PixelToUnit(Screen.width) / 2) + (gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2);
                transform.position = new Vector2(leftBoundaryAligned, transform.position.y);
            }

        }

        if (mainCamera.ShipAtCorrectYPosition() && Camera.main.WorldToScreenPoint(transform.position).x + (shipPixelWidth / 2) < Screen.width - 0.1) {

            if (touchAvailable) {
                MoveRightWithTouch();
            } else {
                MoveRightWithInput();

            }

            if (Camera.main.WorldToScreenPoint(transform.position).x + (shipPixelWidth / 2) > Screen.width) {
                float rightBoundaryAligned = (Helper.PixelToUnit(Screen.width) / 2) - (gameObject.GetComponent<MeshRenderer>().bounds.size.x / 2);
                transform.position = new Vector2(rightBoundaryAligned, transform.position.y);
            }

        }

    }

    private void MoveLeftWithInput() {

        if (Input.GetAxisRaw("Horizontal") < 0) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - transform.localScale.x, transform.position.y), horizontalSpeed * Time.deltaTime);
        }

    }

    private void MoveRightWithInput() {

        if (Input.GetAxisRaw("Horizontal") > 0) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + transform.localScale.x, transform.position.y), horizontalSpeed * Time.deltaTime);
        }

    }



    private void MoveLeftWithTouch() {

        if(Input.touchCount > 0 && Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x < transform.position.x) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, transform.position.y), horizontalSpeed * Time.deltaTime);
        }

    }

    private void MoveRightWithTouch() {

        if (Input.touchCount > 0 && Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x > transform.position.x) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, transform.position.y), horizontalSpeed * Time.deltaTime);
        }

    }

    private void IncreaseSpeed() {

        if(verticalSpeed >= verticalSpeedAccelerationGoal) {
            reaccelerating = false;
        }

        if(reaccelerating) {
            verticalSpeed += (verticalSpeedIncreaseIncrement * (GetCurrentPosition() * 0.08f)) * Time.deltaTime;
        } else {
            verticalSpeed += verticalSpeedIncreaseIncrement * Time.deltaTime;
        }
        
    }

    private void AdjustSlowdownField() {
        Vector2[] points = slowDownField.points;
        points[0].y = Mathf.Clamp(originalLengthOfSlowndownField + GetDividedSpeedPercentage(0.3f), 1.5f, 2f);
        slowDownField.points = points;
    }

    private float GetDividedSpeedPercentage(float unitToDivide) {
        return ((GetCurrentSpeedPercentage() - 100f) / 100f) * unitToDivide;
    }

}

