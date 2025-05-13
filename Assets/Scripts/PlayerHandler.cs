using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public float multiplier = 10f;
    private ConstantForce Force;

    void Start()
    {
        Force = GetComponent<ConstantForce>();
    }

    // Update is called once per frame
    void Update()
    {
        float VelocityX = Input.GetAxis("Horizontal");
        float VelocityY = Input.GetKey(KeyCode.E) ? 1f : Input.GetKey(KeyCode.Q) ? -1f : 0f;
        float VelocityZ = Input.GetAxis("Vertical");

        Vector3 ForceDirection = transform.TransformDirection(new Vector3(VelocityX, VelocityY, VelocityZ) * multiplier);
        Debug.Log(ForceDirection);
        Force.force = ForceDirection;
    }
}
