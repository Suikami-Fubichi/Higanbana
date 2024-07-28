using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject potion;
    private Vector3 initPos;
    public float amp, freq;
    public float healAmount;

    private PlayerStats stats;
    private void Start()
    {
        initPos = potion.transform.position;
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    private void Update()
    {
        potion.transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stats.Heal(healAmount);
            Destroy(gameObject);
        }
    }
   
}
