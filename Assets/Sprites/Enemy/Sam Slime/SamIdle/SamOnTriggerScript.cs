using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamOnTriggerScript : MonoBehaviour
{
    private Animator anim;
    public GameObject slime;
    private void Start()
    {
        anim = slime.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter");
        anim.SetTrigger("damage");
    }
}
