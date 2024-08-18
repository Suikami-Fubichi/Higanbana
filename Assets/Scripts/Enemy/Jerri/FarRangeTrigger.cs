using UnityEngine;

public class FarRangeTrigger : MonoBehaviour
{
    private JerriController parent;

    void Start()
    {
        parent = gameObject.GetComponentInParent<JerriController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !parent.GetIsAttacking())
        {
            int ran = Random.Range(1, 10);
            if (ran <= 5)
            {
                parent.TriggerJumpShot();
            }
            else
            {
                parent.TriggerRain();
            }

        }
    }
}
