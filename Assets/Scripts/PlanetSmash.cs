using TMPro;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI elements
using System.Collections.Generic;
public class PlanetSmash : MonoBehaviour
{
    public TextMeshProUGUI planetsSmashedLabel; // Reference to the UI Text element that displays the number of smashed planets
    public TextMeshProUGUI planetsSmashedDeathLabel;
    private int planetsSmashed = 0; // Counter for smashed planets
    private Vector3 position;
    private int highestScore;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public GameObject explosion;
    [SerializeField]
    public PlayerHandler playerHandler;
    private void Start()
    {
        // Initialize the label
        UpdatePlanetsSmashedLabel();
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        else
        {
            highestScore = PlayerPrefs.GetInt("HighScore");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is a planet
        if (collision.gameObject.CompareTag("Planet"))
        {
            audioManager.PlaySFX(audioManager.PlanetDestroy);
            // Increase the planets smashed count
            planetsSmashed++;
            // Update the UI label
            UpdatePlanetsSmashedLabel();
            position = collision.gameObject.transform.position;
            // Destroy the planet
            collision.gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(collision.gameObject, 1);
            GameObject clone = Instantiate(explosion, position, Quaternion.identity);// as GameObject;
            Destroy(clone, 5);
        }
        else
        {

        }
    }

    private void UpdatePlanetsSmashedLabel()
    {
        // Update the text of the label to show the current count
        planetsSmashedLabel.text = "Planets Smashed: " + planetsSmashed;
        planetsSmashedDeathLabel.text = "Planets Smashed: " + planetsSmashed;
        playerHandler.SetFuel(100f, true);
    }
    public int GetPlanetsSmashed()
    {
        return planetsSmashed;
    }
}