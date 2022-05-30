using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites; // when mario runs we cycle through these run sprites in the array
    public Sprite climbSprite; // these two are public because we assign them in the editor
    private int spriteIndex; // keep track of what sprite we are currently on via index from sprite array

    private new Rigidbody2D rigidbody; // new is a new variable, rather than override the built in old existing rigidbody    private Vector2 direction;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f; // how high can mario jump

    private bool grounded;
    private bool climbing;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>(); // looks for a component that matches the name in Unity
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4]; // sets max number of things to collide/overlap with
    }

    private void OnEnable() // when Mario is enabled we can invoke a function repeatedly
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f); // gets called every 12th of a second, repeats every 12th of a second
    }

    private void OnDisable() 
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {   
        grounded = false; // assume we are not grounded, every frame we are initially not grounded and set to true if we overlap with the ground
        climbing = false;

        Vector2 size = collider.bounds.size;  
        size.y += 0.1f; // this is changing the collider size so we are always colliding with ground
        size.x /= 2f; // this makes it so to collide with a ladder and use it we stand more inline with it by having a narrower collider

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++) // i < amount of objects we overlap with
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground")) // if we have touched the ground
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f); // is the ground we just hit below us, and moves collider down .5 closer to feet

                Physics2D.IgnoreCollision(collider, results[i], !grounded); // if grounded = false we can do the opposite. ignore collision between mario's collider, object we are overlapping with and if not grounded
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder")) // climbing a ladder
            {
                climbing = true;
            }
            
        }
    }

    private void Update() 
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed; // climbing a ladder
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        } 
        else 
        {
            direction += Physics2D.gravity * Time.deltaTime; // spreads gravity force out over time, instead of each frame, + consistent regardless of framerate
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (grounded) {
            direction.y = Mathf.Max(direction.y, -1f); // direction.y is the max of whatever current direction.y is and -1, So direction.y will never exceed -1. Stops gravity compounding
        }

        if (direction.x > 0f) // indicates moving to the right
        {
            transform.eulerAngles = Vector3.zero; // character is already facing right
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); // x = 0, y = 180, z = 0
        }
        
    }

    private void FixedUpdate() 
    {   // take current position then add current direction then multiply
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime); 
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++; // cycle through array

            if (spriteIndex >= runSprites.Length) // if we get to end of array we want to loop back through
            {
                spriteIndex = 0; // if exceeded length of array then set back to 0
            }

            spriteRenderer.sprite = runSprites[spriteIndex]; // assign whatever sprite is in the array at that index ?
        }
    }
}
