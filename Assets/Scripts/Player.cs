using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public float speed;
    public float detectDistance;
    public GameObject ArrowPrefab;
    public Vector3 initialPosition;
    //public Animation attack;
    //private Animator animator;

    private bool isMoving;
    private bool up;
    private bool down;
    private bool left;
    private bool right;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 currentPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //人物技能激活状态
    public enum Player_State
    {
        Walk,
        Sword,
        Bow,
        Bomb,   //催城
        Dead,
    };
    public Player_State player_state;

    public enum Direction
    {
        Left,
        Right
    }
    public Direction direction;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player_state = Player_State.Walk;
        direction = Direction.Left;
        this.transform.position = initialPosition;
        currentPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void MoveControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player_state = Player_State.Walk;
            // 需要相应的UI动画，激活功能，下同
            // TODO
        }

        if (isMoving)
            return;


        if (player_state == Player_State.Bow)
        {//射箭时不会移动
            BowAttack();
            return;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            up = true;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            down = true;
            isMoving = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            left = true;
            isMoving = true;
            direction = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            right = true;
            isMoving = true;
            direction = Direction.Right;
        }
    }

    private void Move()
    {
        //人物移动
        if (isMoving == true)
        {
            if (left == true)
            {
                if (transform.position.x != currentPosition.x - 1)
                {
                    Vector2 target = new Vector2(currentPosition.x - 1, currentPosition.y);
                    rb.MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));
                }
                else
                {
                    currentPosition.x -= 1;
                }
            }
            else if (right == true)
            {
                if (transform.position.x != currentPosition.x + 1)
                {
                    Vector2 target = new Vector2(currentPosition.x + 1, currentPosition.y);
                    rb.MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));
                }
                else
                {
                    currentPosition.x += 1;
                }
            }
            else if (up == true)
            {
                if (transform.position.y != currentPosition.y + 1)
                {
                    Vector2 target = new Vector2(currentPosition.x, currentPosition.y + 1);
                    rb.MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));
                }
                else
                {
                    currentPosition.y += 1;
                }
            }
            else if (down == true)
            {
                if (transform.position.y != currentPosition.y - 1)
                {
                    Vector2 target = new Vector2(currentPosition.x, currentPosition.y - 1);
                    rb.MovePosition(Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime));
                }
                else
                {
                    currentPosition.y -= 1;
                }
            }
        }
        //人物朝向
        if (direction == Direction.Left)
        {
            sr.flipX = false;
        }
        else if (direction == Direction.Right)
        {
            sr.flipX = true;
        }
        //检测是否需要停下
        if (right == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.right, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                isMoving = false;
                right = false;
                this.transform.position = currentPosition;
            }
        }
        else if (left == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.left, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                isMoving = false;
                left = false;
                this.transform.position = currentPosition;
            }
        }
        else if (up == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.up, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                isMoving = false;
                up = false;
                this.transform.position = currentPosition;
            }
        }
        else if (down == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.down, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                isMoving = false;
                down = false;
                this.transform.position = currentPosition;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "DestroyableWall" && player_state == Player_State.Bomb)
        {
            BombAttack(collision);
            return;
        }
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Enemy" || collision.collider.tag == "DestroyableWall")
        {
            if (up == true)
            {
                this.transform.position = currentPosition;
                up = false;
            }
            if (down == true)
            {
                this.transform.position = currentPosition;
                down = false;
            }
            if (left == true)
            {
                this.transform.position = currentPosition;
                left = false;
            }
            if (right == true)
            {
                this.transform.position = currentPosition;
                right = false;
            }
            isMoving = false;
        }
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("tag");
            if (player_state == Player_State.Sword)
            {
                Debug.Log("state");
                SwordAttack(collision);
                player_state = Player_State.Walk;  // 砍完一次怪就会失去剑
            }
            else Die();
        }




    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SwordCard")
        {
            Destroy(collision.gameObject);
            SwordCardManager.Instance.PickUpCard();
        }

        if (collision.tag == "BowCard")
        {
            Destroy(collision.gameObject);
            BowCardManager.Instance.PickUpCard();
        }
    }

    public void SwordAttack(Collision2D collision)
    {
        //播放砍人动画 
        //TODO
        Debug.Log("SwordAttack");
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        enemy.Die();
    }
    public void BowAttack()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = new Vector2(-1, 0);
            direction = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = new Vector2(1, 0);
            direction = Direction.Right;
        }

        if (dir != Vector2.zero)
        {
            // 播放动画
            GameObject arrow = Instantiate(ArrowPrefab, this.transform.position, Quaternion.identity);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            arrowController.Move(dir);
            player_state = Player_State.Walk;
        }


    }
    private void BombAttack(Collision2D collision)
    {
        //播放摧城动画
        DWallController wall = collision.collider.GetComponent<DWallController>();
        wall.Bomb();
    }
    public void Die()
    {
        //animator.SetBool("Dead", true); // 播放死亡动画
        player_state = Player_State.Dead;
    }
}


