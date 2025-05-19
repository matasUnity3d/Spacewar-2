using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Attraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    private Vector3 position;
    public GameObject explosion;
    public Canvas deathScreen;
    public Canvas mainUI;
    private GameObject fighter;
    public TextMeshProUGUI HighScoreLabel;
    public float baseAttractionForce = 100000f; // Base force of attraction
    public float deathDistance = 1f; // Distance at which the player dies
    public Camera mainCamera;
    public Camera deathCamera;
    private bool died = false;

    [SerializeField]
    public PlayerHandler playerHandler;
    public PlanetSmash planetSmash;
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
                Die();
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

    private void Die()
    {
        
        if (!died)
        {
            audioManager.PlaySFX(audioManager.Death);
            int savedHighScore = PlayerPrefs.GetInt("HighScore");
            int planetSmashLocal = planetSmash.GetPlanetsSmashed();
            if (savedHighScore < planetSmashLocal)
            {
                HighScoreLabel.text = "NEW HighScore: " + planetSmashLocal;
                PlayerPrefs.SetInt("HighScore", planetSmashLocal);
            }
            else
            {
                HighScoreLabel.text = "HighScore: " + savedHighScore;
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
            died = true;
        }
    }
}
