using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPlusController : MonoBehaviour
{
    private float moveForce;
    Rigidbody2D rbody;
    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        moveForce = 800f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 控制箭的移动
    public Vector2 direction = Vector2.zero;
    public void Move(UnityEngine.Vector2 moveDirection)
    {
        direction = moveDirection;
        rbody.AddForce(moveDirection * moveForce);
        // 计算射击方向的角度
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // 将角度应用旋转
        this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            return;
        if (collision.collider.tag == "Enemy")
        {
            EnemyController enemy = collision.collider.GetComponent<EnemyController>();
            enemy.Die();
            Move(direction);
        }

        if (collision.collider.tag == "Wall" || collision.collider.tag == "DestroyableWall")
            Destroy(gameObject);
    }
}
