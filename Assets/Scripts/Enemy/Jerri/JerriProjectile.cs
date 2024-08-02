using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JerriProjectile : MonoBehaviour
{
    [SerializeField]
    private float damage, speed;

    private Rigidbody2D rb;

    private PlayerController pc;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Fly();
    }

    private void Fly()
    {
        //rb.velocity = new Vector2(speed * direction.x, direction.y);
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pc.GetDashStatus() && collision.gameObject.tag == "Player")
        {
            float[] attackDetails = new float[2];
            attackDetails[0] = damage;
            attackDetails[1] = rb.transform.position.x;
            collision.gameObject.SendMessage("Damage", attackDetails);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
