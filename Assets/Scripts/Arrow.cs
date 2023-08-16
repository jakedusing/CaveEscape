using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 20f;
    [SerializeField] int pointsForEnemyDestroy = 50;
    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
        transform.localScale = new Vector2((Mathf.Sign(xSpeed)) * transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            FindObjectOfType<GameSession>().AddToScore(pointsForEnemyDestroy);
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
