using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPerimeter : MonoBehaviour
{
    [SerializeField]
    private GameObject boss;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { 
            boss.SendMessage("Wake", collision);
        }
    }
}
