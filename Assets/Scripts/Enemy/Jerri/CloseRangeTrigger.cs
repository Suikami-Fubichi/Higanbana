using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeTrigger : MonoBehaviour
{
    private JerriController parent;

    void Start()
    {
        parent = gameObject.GetComponentInParent<JerriController>();
    }

   /* private void OnTriggerEnter2D(Collider2D collision )
    {
        if (collision.gameObject.tag == "Player" && !parent.GetIsAttacking())
        {
            int ran = Random.Range(1, 10);
            if (ran <= 5)
            {
                parent.TriggerMelee();
            }
            else
            {
                parent.TriggerBackup();
            }
            
        }
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !parent.GetIsAttacking())
        {
            int ran = Random.Range(1, 10);
            if (ran <= 5)
            {
                parent.TriggerMelee();
            }
            else
            {
                parent.TriggerBackup();
            }

        }
    }
}
