using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //简单的相机移动，使用方向键控制
    public bool isEnabled;
    public float speed = 100f;
    float moveX;
    float moveY;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        moveX = 0;
        moveY = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;
        Move();
    }
    void Move()
    {
        if (Player.Instance.isMoving)
        {
            this.transform.position = Player.Instance.transform.position;
            return;
        }

        moveX = 0;
        moveY = 0;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1;
        }

        if (moveX != 0 && moveY != 0)
        {
            moveX /= (float)System.Math.Sqrt(2);
            moveY /= (float)System.Math.Sqrt(2);
        }

        Vector2 position = this.transform.position;
        position.x += moveX * speed * Time.deltaTime;
        position.y += moveY * speed * Time.deltaTime;
        this.transform.position = position;
    }
}
