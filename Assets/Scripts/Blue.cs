using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : MonoBehaviour
{
    private Rigidbody rb;
    public Transform player;
    private Vector3 movement;
    // Start is called before the first frame update
    public float moveSpeed = 15f;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        Debug.Log(direction);
        direction.Normalize();
        movement=direction;
    }

    void moveCharacter(Vector3 direction){
         rb.MovePosition((Vector3)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }
}
