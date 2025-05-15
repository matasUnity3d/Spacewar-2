using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FuelUI : MonoBehaviour
{
    public float Fuel, MaxFuel, Width, Height;
    [SerializeField]
    private RectTransform fuelBar;
    // Fancy logic stuff to manipulate the fuel bar
    public void SetMaxFuel(float maxFuel){
        MaxFuel = maxFuel;
    }

    public void SetFuel(float fuel){
        Fuel = fuel;
        float newWidth = (Fuel / MaxFuel) * Width;

        fuelBar.sizeDelta = new Vector2(newWidth, Height);
    }

}
