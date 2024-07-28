using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text hpText;


    public void UpdateHealth(float amount, float maxHP)
    {
        healthSlider.value = amount;
        if(amount < 0)
        {
            amount = 0;
        }
        string text = (amount + "/" + maxHP);
        hpText.SetText(text);
    }
}
