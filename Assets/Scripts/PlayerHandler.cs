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

    private float yaw; // Yaw rotation based on mouse movement
    private float pitch; // Pitch rotation based on mouse movement
    public float pitchLimit = 360f; // Limit for pitch rotation

    // Define the map boundaries
    public Vector3 mapCenter = Vector3.zero; // Center of the map
    public Vector3 mapDimensions = new Vector3(1000f, 1000f, 1000f); // Size of the map

    void Start()
    {
        Force = GetComponent<ConstantForce>();
        fwd.enableEmission = false;
        back.enableEmission = false;
        left.enableEmission = false;
        right.enableEmission = false;
        down.enableEmission = false;
        up.enableEmission = false;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        SetEmission("w", fwd);
        SetEmission("s", back);
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
        Vector3 ForceDirection = transform.TransformDirection(new Vector3(VelocityX, VelocityY, VelocityZ) * multiplier);
        Force.force = ForceDirection;

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

    void SetEmission(string key, ParticleSystem particleSystem)
    {
        var emission = particleSystem.emission;
        if (Input.GetKeyDown(key)) emission.enabled = true;
        if (Input.GetKeyUp(key)) emission.enabled = false;
    }

    void SetEmission(KeyCode key, ParticleSystem particleSystem)
    {
        var emission = particleSystem.emission;
        if (Input.GetKeyDown(key)) emission.enabled = true;
        if (Input.GetKeyUp(key)) emission.enabled = false;
    }
}
