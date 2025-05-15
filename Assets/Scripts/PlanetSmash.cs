using TMPro;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI elements

public class PlayerScript : MonoBehaviour
{
    public TextMeshProUGUI planetsSmashedLabel; // Reference to the UI Text element that displays the number of smashed planets
    private int planetsSmashed = 0; // Counter for smashed planets

    [SerializeField]
    public PlayerHandler playerHandler;
    private void Start()
    {
        // Initialize the label
        UpdatePlanetsSmashedLabel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is a planet
        if (collision.gameObject.CompareTag("Planet"))
        {
            // Increase the planets smashed count
            planetsSmashed++;
            // Update the UI label
            UpdatePlanetsSmashedLabel();

            // Destroy the planet
            Destroy(collision.gameObject);
        }
    }

    private void UpdatePlanetsSmashedLabel()
    {
        // Update the text of the label to show the current count
        planetsSmashedLabel.text = "Planets Smashed: " + planetsSmashed;
        playerHandler.SetFuel(100f, true);
    }
}
