using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public float multiplier = 10f;
    public float yawSpeed = 100f;
    public float mouseSensitivity = 2f; // Sensitivity for mouse movement
    private ConstantForce Force;
    public ParticleSystem left;
    public ParticleSystem right;
    public ParticleSystem back;
    public ParticleSystem fwd;
    public ParticleSystem up;
    public ParticleSystem down;
    public ParticleSystem yawLeft;
    public ParticleSystem yawRight;
    public ParticleSystem timeWarp;
    public float Fuel, MaxFuel;
    public float fuelDrain = -10000f;
    public bool moving = false;
    public Camera myCamera;
    private bool hasFuel = true;
    [SerializeField]
    private FuelUI fuel;
    private float yaw; // Yaw rotation based on mouse movement
    private float pitch; // Pitch rotation based on mouse movement
    public float pitchLimit = 360f; // Limit for pitch rotation
    private float fov = 90;
    private float t = 0.5f;
    // Define the map boundaries
    public Vector3 mapCenter = Vector3.zero; // Center of the map
    public Vector3 mapDimensions = new Vector3(1000f, 1000f, 1000f); // Size of the map
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        fuelDrain /= 100;
        DisableEmissions();
        fuel.SetMaxFuel(MaxFuel);
        Force = GetComponent<ConstantForce>();
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFuel)
        {
            TimeWarp();
            SetFuel(fuelDrain, false);
            SetEmission(KeyCode.W, fwd);
            SetEmission(KeyCode.S, back);
            SetEmission(KeyCode.A, left);
            SetEmission(KeyCode.D, right);
            SetEmission(KeyCode.LeftControl, down);
            SetEmission(KeyCode.LeftShift, up);
            // Mouse-based turning
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            yaw += mouseX; // Update yaw based on mouse movement
            pitch -= mouseY; // Update pitch based on mouse movement
            // pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit); // Uncomment if you want to clamp pitch

            // Apply rotation
            transform.eulerAngles = new Vector3(pitch, yaw, 0);

            // Emission based on mouse movement
            yawLeft.enableEmission = mouseX != 0;
            yawRight.enableEmission = mouseY != 0;

            // Movement input
            float VelocityX = Input.GetAxis("Horizontal");
            float VelocityY = Input.GetKey(KeyCode.LeftShift) ? 1f : Input.GetKey(KeyCode.LeftControl) ? -1f : 0f;
            float VelocityZ = Input.GetAxis("Vertical");
            SetMovement(VelocityX, VelocityY, VelocityZ);

            moving = IsAnyMovementKeyPressed();
            if (moving)
            {
                //audioManager.PlaySFX(audioManager.Boost);
            }
        }
        else
        {
            audioManager.PlaySFX(audioManager.FuelUp);
            Debug.Log("Death");
            moving = false;
            DisableEmissions();
            SetMovement(0, 0, 0);
        }
        // Check for map borders
        CheckMapBorders();
    }

    void CheckMapBorders()
    {
        Vector3 position = transform.position;

        // Check if the player is outside the map boundaries
        if (position.x < mapCenter.x - mapDimensions.x / 2) position.x = mapCenter.x + mapDimensions.x / 2;
        else if (position.x > mapCenter.x + mapDimensions.x / 2) position.x = mapCenter.x - mapDimensions.x / 2;

        if (position.y < mapCenter.y - mapDimensions.y / 2) position.y = mapCenter.y + mapDimensions.y / 2;
        else if (position.y > mapCenter.y + mapDimensions.y / 2) position.y = mapCenter.y - mapDimensions.y / 2;

        if (position.z < mapCenter.z - mapDimensions.z / 2) position.z = mapCenter.z + mapDimensions.z / 2;
        else if (position.z > mapCenter.z + mapDimensions.z / 2) position.z = mapCenter.z - mapDimensions.z / 2;

        // Update the player's position
        transform.position = position;
    }

    public void SetHasFuel(bool fuel)
    {
        hasFuel = fuel;
    }
    public void SetFov(float newFov)
    {
        myCamera.fieldOfView = newFov;
    }

    void SetEmission(KeyCode key, ParticleSystem particleSystem)
    {
        var emission = particleSystem.emission;
        if (Input.GetKeyDown(key)){
            if(key == KeyCode.W){
                fov = 115f;
                timeWarp.enableEmission = true;
            }
            emission.enabled = true;
        }
        if (Input.GetKeyUp(key)){
            if(key == KeyCode.W){
                fov = 90f;
                timeWarp.enableEmission = false;
            }
            emission.enabled = false;
        }
    }
    // Update is needed because you can collide with planets without boosting, thus not updating the fuel
    // Only Planet smasher calls this with true
    public void SetFuel(float fuelChange, bool update){
        if(moving || update){
            if((fuelChange + Fuel) >= MaxFuel){
                Fuel=MaxFuel;
            }
            else {
                Fuel += fuelChange;
                //Fuel = Mathf.Clamp(Fuel, 0, MaxFuel);
            }
            Debug.Log("Moving");
            fuel.SetFuel(Fuel);
        }
        if(Fuel <= 0){
            hasFuel = false;
        }
    }
    void DisableEmissions(){
        timeWarp.enableEmission = false;
        fwd.enableEmission = false;
        back.enableEmission = false;
        left.enableEmission = false;
        right.enableEmission = false;
        down.enableEmission = false;
        up.enableEmission = false;
    }

    public void SetMovement(float x, float y, float z){
        Vector3 ForceDirection = transform.TransformDirection(new Vector3(x, y, z) * multiplier);
        Force.force = ForceDirection;
    }
    bool IsAnyMovementKeyPressed()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || 
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift);
    }
    void TimeWarp()
    {
        if(fov == 115 && myCamera.fieldOfView != fov){
            myCamera.fieldOfView += t;
        }
        else if (fov == 90 && myCamera.fieldOfView != fov){
            myCamera.fieldOfView -= t;
        }
    }
}
