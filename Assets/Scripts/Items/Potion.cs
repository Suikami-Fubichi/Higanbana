using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    // Public variables to control potion behavior
    public GameObject potion;  // Reference to the potion game object
    private Vector3 initPos;   // Initial position of the potion
    public float amp, freq;    // Amplitude and frequency for potion's floating animation
    public float healAmount;   // Amount of health the potion heals

    // Reference to the player's stats
    private PlayerStats stats;

    // Initialize variables
    private void Start()
    {
        // Store the initial position of the potion
        initPos = potion.transform.position;
        // Get the PlayerStats component from the player object
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame to handle the potion's floating effect
    private void Update()
    {
        // Apply a floating animation by adjusting the potion's Y position using a sine wave
        potion.transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, 0);
    }

    // Triggered when the potion collides with another collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.tag == "Player")
        {
            // Heal the player by the specified amount
            stats.Heal(healAmount);
            // Destroy the potion after it is used
            Destroy(gameObject);
        }
    }
}
