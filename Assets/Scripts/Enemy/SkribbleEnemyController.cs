using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkribbleEnemyController : MonoBehaviour
{
    private float spawnTimer = 3f;
    private float timeLeft;
    private bool isSpawning = false;
    public GameObject projectile;
    public LayerMask playerMask;
    public float detectRadius = 10f;
    public float currentHealth = 100f;

    public GameObject hitParticle;

    public Transform spawn;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = spawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawnTime();
    }

    void CheckSpawnTime()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            CheckPlayerOverlap();
        }
    }

    void SpawnProjectile()
    {
        timeLeft = spawnTimer;
        Instantiate(projectile, spawn);
    }

    private void CheckPlayerOverlap()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(spawn.position, detectRadius, playerMask);
        foreach (Collider2D collider in detectedObjects)
        {
            SpawnProjectile();
        }
    }

    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];
        Instantiate(hitParticle, spawn.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));


        if (currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(spawn.position, detectRadius);
    }

}
