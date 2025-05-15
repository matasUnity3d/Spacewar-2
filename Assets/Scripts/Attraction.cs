using UnityEngine;
using UnityEngine.SceneManagement;

public class Attraction : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float baseAttractionForce = 100000f; // Base force of attraction
    public float deathDistance = 1f; // Distance at which the player dies
    private void Start()
    {
        deathDistance = 100f;
    }
    private void Update()
    {
        // Check if the player is assigned
        if (!player) return;
        // Calculate the distance to the player
        float distance = Vector3.Distance(player.position, transform.position);
        // Calculate the attraction force using the inverse square law
        float attractionForce = baseAttractionForce / (distance * distance);

        // Calculate the direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Apply attraction force
        player.GetComponent<Rigidbody>().AddForce(direction * -attractionForce);

        // Check for death condition
        if (distance < deathDistance)
        {
            Die();
        }
    }

    private void Die()
    {
        // Reload the current scene
        Debug.Log("Player has died! Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
