using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    //public Animation attack;

    private bool isMoving;
    private bool up;
    private bool down;
    private bool left;
    private bool right;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        Move();
        Stable();
    }

    private void MoveControl()
    {
        if (Input.GetKeyDown(KeyCode.W)&&isMoving==false)
        {
            up = true;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) && isMoving == false)
        {
            down = true;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && isMoving == false)
        {
            left = true;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && isMoving == false)
        {
            right = true;
            isMoving = true;
        }
    }

    private void Move()
    {
        if (up == true)
        {
            this.transform.Translate(Vector2.up * Time.deltaTime * speed, Space.World);
        }
        else if (down == true)
        {
            this.transform.Translate(Vector2.down * Time.deltaTime * speed, Space.World);
        }
        else if (left == true)
        {
            this.transform.Translate(Vector2.left * Time.deltaTime * speed, Space.World);
        }
        else if (right == true)
        {
            this.transform.Translate(Vector2.right * Time.deltaTime * speed, Space.World);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall"||collision.collider.tag=="Enemy")
        {
            up = false;
            down = false;
            left = false;
            right = false;
            isMoving = false;
        }
    }

    private void Stable()
    {
        if (isMoving == false)
        {
            this.transform.position=new Vector3( math.round(this.transform.position.x), math.round(this.transform.position.y),0);
            this.transform.eulerAngles = Vector3.zero;
        }
    }

    private void Attack()
    {
        
    }
}
