using UnityEngine;
using System.Collections;

public class RaycastGun : MonoBehaviour
{
    public GameObject FirePoint;
    public Transform laserOrigin;
    public float gunRange = 50f;
    public float laserDuration = 0.05f;
    AudioManager audioManager;
    LineRenderer laserLine;
    void Start()
    {
        laserLine.enabled = false;
    }

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
 
    void Update()
    {

        if(Input.GetKeyDown("space") && laserLine.enabled == false )
        {
            Debug.Log("Firing!");
            audioManager.PlaySFX(audioManager.Lazer);
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = FirePoint.transform.position;
            RaycastHit hit;
            if(Physics.Raycast(rayOrigin, FirePoint.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                if(hit.collider.gameObject.CompareTag("Enemy")){
                    Destroy(hit.transform.gameObject);
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (FirePoint.transform.forward * gunRange));
            }
            StartCoroutine(ShootLaser());
        }
    }
 
    IEnumerator ShootLaser()
    {
        Debug.Log("Firing laser");
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
