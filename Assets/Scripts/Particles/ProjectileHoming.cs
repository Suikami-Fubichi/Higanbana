using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileHoming : MonoBehaviour
{
    public Transform target;


    public float speed;
    public float rotateSpeed;

    private float[] attackDetails = new float[2];

    private Rigidbody2D rb;

    private PlayerController pc;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        target = GameObject.Find("Player").transform;
    }

    public void UpdatePlayer()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pc.GetDashStatus() && collision.gameObject.tag == "Player")
        {
            attackDetails[0] = 10;
            attackDetails[1] = rb.transform.position.x;
            collision.SendMessage("Damage", attackDetails);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
