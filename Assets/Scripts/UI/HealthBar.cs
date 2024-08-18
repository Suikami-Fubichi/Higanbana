using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    // Public references to the UI components
    public Slider healthSlider;   // Slider component for displaying health
    public TMP_Text hpText;       // Text component for displaying health values

    // Method to update the health bar and text display
    public void UpdateHealth(float amount, float maxHP)
    {
        // Update the slider's value to reflect the current health amount
        healthSlider.value = amount;

        // Ensure the health amount does not go below 0
        if (amount < 0)
        {
            amount = 0;
        }

        // Create a string showing the current health over the maximum health
        string text = (amount + "/" + maxHP);

        // Update the text display with the health information
        hpText.SetText(text);
    }
}
