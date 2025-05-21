using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System;
public class Attraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    private Vector3 position;
    public GameObject explosion;
    public Canvas deathScreen;
    public Canvas mainUI;
    private GameObject fighter;
    public TextMeshProUGUI perished;
    public TextMeshProUGUI PlanetHighScoreLabel;
    public TextMeshProUGUI KillHighScoreLabel;
    public float baseAttractionForce = 100000f; // Base force of attraction
    public float deathDistance = 1f; // Distance at which the player dies
    public Camera mainCamera;
    public Camera deathCamera;
    private bool died = false;

    [SerializeField]
    public PlayerHandler playerHandler;
    public PlanetSmash planetSmash;
    public RaycastGun rayCastGun;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        mainCamera.enabled = true;
        deathCamera.enabled = false;
        deathDistance = 90f;
        deathScreen.enabled = false;
        mainUI.enabled = true;
    }
    private void Update()
    {
        if (!died)
        {
            // Check if the player is assigned
            if (!player.transform) return;
            // Calculate the distance to the player
            float distance = Vector3.Distance(player.transform.position, transform.position);
            // Calculate the attraction force using the inverse square law
            float attractionForce = baseAttractionForce / (distance * distance);

            // Calculate the direction towards the player
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Apply attraction force

            player.transform.GetComponent<Rigidbody>().AddForce(direction * -attractionForce);


            // Check for death condition
            if (distance < deathDistance)
            {
                Die("The sun has swallowed you!");
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void Die(String Reason = "You have perished!")
    {
        
        if (!died)
        {
            Debug.Log("Death");
            
            perished.text = Reason + "\nPress space to continue";
            died = true;
            audioManager.PlaySFX(audioManager.Death);
            int savedHighScore = PlayerPrefs.GetInt("HighScore");
            int savedHighScoreKills = PlayerPrefs.GetInt("HighScoreKills");
            int enemiesDestroyed = rayCastGun.GetKills();
            int planetSmashLocal = planetSmash.GetPlanetsSmashed();
            if (savedHighScore < planetSmashLocal)
            {
                PlanetHighScoreLabel.text = "NEW Planet High Score: " + planetSmashLocal;
                PlayerPrefs.SetInt("HighScore", planetSmashLocal);
            }
            else
            {
                PlanetHighScoreLabel.text = "Planet High Score: " + savedHighScore;
            }
            if (savedHighScoreKills < enemiesDestroyed)
            {
                KillHighScoreLabel.text = "NEW Kill Record: " + enemiesDestroyed;
                PlayerPrefs.SetInt("HighScoreKills", enemiesDestroyed);
            }
            else
            {
                KillHighScoreLabel.text = "Kill High Score: " + savedHighScoreKills;
            }
            mainUI.enabled = false;
            deathScreen.enabled = true;
            mainCamera.enabled = false;
            deathCamera.enabled = true;
            playerHandler.SetFov(150f);
            fighter = GameObject.Find("Fighter_01");
            Debug.Log("Smash sun");
            player = GameObject.Find("Spaceship");
            fighter.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            position = player.transform.position;
            player.SetActive(false);
            playerHandler.SetHasFuel(false);
        }
    }
}
