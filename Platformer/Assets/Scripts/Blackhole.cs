using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D circleCollider;
    public GameObject player;
    public Rigidbody2D playerRb;
    float distToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /*if (Physics2D.Raycast(transform.position, player.transform.position, 1f).collider == true)
        {
            playerRb.AddRelativeForce(transform.position - player.transform.position);
        }*/

       
    }
}
