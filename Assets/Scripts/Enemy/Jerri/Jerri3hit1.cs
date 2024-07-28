using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Jerri3hit1 : MonoBehaviour
{
    public float attackRadius;
    private float[] attackDetails = new float[2];

    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private Transform pos;

    private void Start()
    {
        attackDetails[0] = 15;
    }

    public void CheckAttackHitBox()
    {
        Collider2D hit = Physics2D.OverlapCircle(pos.position, attackRadius, whatIsPlayer);
        if (hit != null)
        {
            attackDetails[1] = transform.position.x;
            hit.transform.SendMessage("Damage", attackDetails);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pos.position, attackRadius);
    }
}
