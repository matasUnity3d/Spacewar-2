using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private Camera playerCamera;
    public GameObject planetPrefab;
    public int numberOfPlanets = 100;
    public float spawnDistance = 500f;
    public float minSize = .5f;
    public float maxSize = 30f;
    public float checkInterval = 1f;

    public GameObject enemyPrefab; 
    public int numberOfEnemies = 10;
    public Vector3 enemySpawnPosition = Vector3.zero;

    void Start()
    {
        Physics.gravity = Vector3.zero;
        playerCamera = Camera.main;
        SpawnPlanets();
        SpawnEnemies();
        StartCoroutine(CheckAndSpawnPlanets());
    }

    private IEnumerator CheckAndSpawnPlanets()
    {
        while (true) // Infinite loop to keep checking
        {
            SpawnPlanetsIfNeeded();
            yield return new WaitForSeconds(checkInterval);
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
        Collider[] colliders = FindObjectsOfType<Collider>();
        Debug.Log(colliders.Length);
        Debug.Log(colliders);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Planet"))
            {
                if (GeometryUtility.TestPlanesAABB(planes, collider.bounds))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void SpawnPlanets()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere * spawnDistance;

            Vector3 spawnPosition = transform.position + playerCamera.transform.forward * spawnDistance;

            Vector3 randomPosition = spawnPosition + randomDirection;

            GameObject planet = Instantiate(planetPrefab, randomPosition, Random.rotation);

            Renderer planetRenderer = planet.GetComponent<Renderer>();
            if (planetRenderer != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                planetRenderer.material.color = randomColor;
            }

            planet.tag = "Planet";
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomDirection = Random.onUnitSphere * spawnDistance;

            Vector3 spawnPosition = transform.position + playerCamera.transform.forward * spawnDistance;

            Vector3 randomPosition = spawnPosition + randomDirection;

            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Random.rotation);

            float randomSize = Random.Range(minSize, maxSize);
            enemy.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

            Renderer enemyRenderer = enemy.GetComponent<Renderer>();
            if (enemyRenderer != null)
            {
                Color randomColor = new Color(Random.value, Random.value, Random.value);
                enemyRenderer.material.color = randomColor;
            }
            enemy.tag = "Enemy"; 
        }
    }
}
