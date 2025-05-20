using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
public class RaycastGun : MonoBehaviour
{
    public GameObject FirePoint;
    public Transform laserOrigin;
    public TextMeshProUGUI plusOne;
    public float gunRange = 50f;
    public float laserDuration = 0.05f;
    public TextMeshProUGUI killsLabel;
    public TextMeshProUGUI killsLabelDeath;
    AudioManager audioManager;
    LineRenderer laserLine;
    private int enemiesDestroyed = 0;
    void Start()
    {
        laserLine.enabled = false;
        plusOne.text = "";
    }

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {

        if (Input.GetKeyDown("space") && laserLine.enabled == false)
        {
            Debug.Log("Firing!");
            audioManager.PlaySFX(audioManager.Lazer);
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = FirePoint.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, FirePoint.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    audioManager.PlaySFX(audioManager.Destroyed);
                    Destroy(hit.transform.gameObject);
                    enemiesDestroyed++;
                    Debug.Log("Enemies Destroyed: " + enemiesDestroyed);
                    killsLabel.text = "KILLS: " + enemiesDestroyed;
                    killsLabelDeath.text = "KILLS: " + enemiesDestroyed;
                    StartCoroutine(DisplayKill());
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (FirePoint.transform.forward * gunRange));
            }
            StartCoroutine(ShootLaser());
        }
    }

    public int GetKills()
    {
        return enemiesDestroyed;
    }

    IEnumerator ShootLaser()
    {
        Debug.Log("Firing laser");
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }

    private IEnumerator DisplayKill()
    {
        plusOne.text = "+1";
        yield return new WaitForSeconds(0.2f);
        plusOne.text = "";


    }
}
