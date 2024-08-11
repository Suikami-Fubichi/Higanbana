using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera CVC;

    private HealthBar healthBar;

    private PlayerStats stats;

    private ProjectileHoming projectile;

    private void Start()
    {
        CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        healthBar = GameObject.Find("Health UI").GetComponent<HealthBar>();
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void UpdateHealth(float amount, float maxHP)
    {
        healthBar.UpdateHealth(amount, maxHP);
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
        //player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

    }

    private void CheckRespawn()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            //player.GetComponent<Renderer>().enabled = true;
            player.SetActive(true);
            player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            respawn = false;
            healthBar.UpdateHealth(100f, 100f);
            stats.ResetHP();


            //Old Respawn Func:
            /*
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
            healthBar.UpdateHealth(100f, 100f);
            stats.ResetHP();
            projectile.UpdatePlayer();*/
        }
    }
}
