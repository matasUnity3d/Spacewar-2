using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public float multiplier = 10f;
    public float yawSpeed = 100f;
    private ConstantForce Force;
    public ParticleSystem left;
    public ParticleSystem right;
    public ParticleSystem back;
    public ParticleSystem fwd;
    public ParticleSystem up;
    public ParticleSystem down;
    public ParticleSystem yawLeft;
    public ParticleSystem yawRight;
    
    void Start()
    {
        Force = GetComponent<ConstantForce>();
        fwd.enableEmission = false;
        back.enableEmission = false;
        left.enableEmission = false;
        right.enableEmission = false;
        down.enableEmission = false;
        up.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w")) fwd.enableEmission = true;
        if (Input.GetKeyUp("w")) fwd.enableEmission = false;
        if (Input.GetKeyDown("s")) back.enableEmission = true;
        if (Input.GetKeyUp("s")) back.enableEmission = false;
        if (Input.GetKeyDown("a")) left.enableEmission = true;
        if (Input.GetKeyUp("a")) left.enableEmission = false;
        if (Input.GetKeyDown("d")) right.enableEmission = true;
        if (Input.GetKeyUp("d")) right.enableEmission = false;
        if (Input.GetKeyDown(KeyCode.LeftControl)) down.enableEmission = true;
        if (Input.GetKeyUp(KeyCode.LeftControl)) down.enableEmission = false;
        if (Input.GetKeyDown(KeyCode.LeftShift)) up.enableEmission = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) up.enableEmission = false;
        if (Input.GetKey("q"))
        {
            transform.Rotate(Vector3.up, -yawSpeed * Time.deltaTime);
            yawLeft.enableEmission = true;
        }
        else
        {
            yawLeft.enableEmission = false;
        }

        if (Input.GetKey("e"))
        {
            transform.Rotate(Vector3.up, yawSpeed * Time.deltaTime);
            yawRight.enableEmission = true;
        }
        else
        {
            yawRight.enableEmission = false;
        }
        float VelocityX = Input.GetAxis("Horizontal");
        float VelocityY = Input.GetKey(KeyCode.LeftShift) ? 1f : Input.GetKey(KeyCode.LeftControl) ? -1f : 0f;
        float VelocityZ = Input.GetAxis("Vertical");
        Vector3 ForceDirection = transform.TransformDirection(new Vector3(VelocityX, VelocityY, VelocityZ) * multiplier);
        //Debug.Log(ForceDirection);
        Force.force = ForceDirection;
    }
}