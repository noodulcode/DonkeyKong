using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 1f;

    private void Awake() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) // called automatically by Unity everytime object collides
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Barrier"))
        {
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse); // just a one time force
        }
    }
        
}
