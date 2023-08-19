using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] Transform enemyProjectileSpawn;
    Rigidbody2D myRigidbody;
    BoxCollider2D myCollider;

    private float _fireRate = 3f;
    private float _canFire = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) {
        FlipEnemyFacing(); 
        moveSpeed = -moveSpeed;
           
    }

    void FlipEnemyFacing() {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

    
}
