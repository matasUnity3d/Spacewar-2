using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{

    public TextMeshProUGUI controlsLabel;
    void Start()
    {
        Destroy(controlsLabel, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
