using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Rigidbody2D rb;

    public EnemyEnum enemyType;

    private float velX = 0;
    private float velY = 3;
    private float lastVel = 0;

    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
    }
    private void FixedUpdate()
    {
        if (transform.position.y > 5)
        {
            velY = -3;
        }
        if (transform.position.y < -5)
        {
            velY = 3;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastVel = velY;
            velY = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            velY = lastVel;
        }
    }
}
