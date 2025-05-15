using UnityEngine;

public class PlanetUnloader : MonoBehaviour
{
    public float visibilityDistance = 1500f; // Distance threshold for visibility

    void Update()
    {
        UpdatePlanetVisibility();
    }

    void UpdatePlanetVisibility()
    {
        // Find all planet objects in the scene
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planets)
        {
            // Calculate the distance from the player to the planet
            float distance = Vector3.Distance(transform.position, planet.transform.position);

            // Check if the planet is within the visibility distance
            if (distance > visibilityDistance)
            {
                Destroy(planet);
            }
        }
    }
}
