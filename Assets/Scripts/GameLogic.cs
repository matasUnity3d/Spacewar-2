using System.Collections;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    private Camera playerCamera;
    public GameObject planetPrefab; // Assign your planet prefab in the inspector
    public int numberOfPlanets = 10; // Number of planets to spawn
    public float spawnRadius = 500f; // Radius around the player to spawn planets
    public float spawnDistance = 100f;
    public float minSize = .5f;
    public float maxSize = 30f;
    public float checkInterval = 1f;
    void Start()
    {
        Physics.gravity = Vector3.zero;
        playerCamera = Camera.main; // Get the main camera
        SpawnPlanets();
        StartCoroutine(CheckAndSpawnPlanets());
    }

    private IEnumerator CheckAndSpawnPlanets()
    {
        while (true) // Infinite loop to keep checking
        {
            SpawnPlanetsIfNeeded();
            yield return new WaitForSeconds(checkInterval); // Wait for the specified interval
        }
    }


    void SpawnPlanetsIfNeeded()
    {
        if (!AreAnyPlanetsVisible())
        {
            SpawnPlanets();
        }
    }
        bool AreAnyPlanetsVisible()
    {
        // Get the planes of the camera's frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        Collider[] colliders = FindObjectsOfType<Collider>(); // Get all colliders in the scene

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Planet")) // Ensure the collider is a planet
            {
                if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
                {
                    return true; // A planet is visible
                }
            }
        }
        return false; // No planets are visible
    }

    void SpawnPlanets()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            // Generate a random direction on the surface of a sphere
            Vector3 randomDirection = Random.onUnitSphere;

            Vector3 spawnPosition = transform.position + playerCamera.transform.forward * spawnDistance;

            // Offset the spawn position by a random direction on the sphere's surface
            Vector3 randomPosition = spawnPosition + randomDirection * spawnRadius;

            // Instantiate the planet prefab
            GameObject planet = Instantiate(planetPrefab, randomPosition, Random.rotation);

            // Randomize the size of the planet
            float randomSize = Random.Range(minSize, maxSize); // Adjust size range as needed
            planet.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

            // Randomize the color of the planet's material
            Renderer planetRenderer = planet.GetComponent<Renderer>();
            if (planetRenderer != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                planetRenderer.material.color = randomColor;
            }

            // Tag the planet for visibility checking
            planet.tag = "Planet";
        }
    }
}
