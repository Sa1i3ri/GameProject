using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public float speed;
    public GameObject ArrowPrefab;
    //public Animation attack;
    //private Animator animator;

    private bool isMoving;
    private bool up;
    private bool down;
    private bool left;
    private bool right;
    private SpriteRenderer sr;

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
        player_state = Player_State.Walk;
        direction = Direction.Left;
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
        if (direction == Direction.Left)
        {
            sr.flipX = false;
        }
        else if (direction == Direction.Right)
        {
            sr.flipX = true;
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
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0);
                up = false;
            }
            if (down == true)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, 0);
                down = false;
            }
            if (left == true)
            {
                this.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y, 0);
                left = false;
            }
            if (right == true)
            {
                this.transform.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, 0);
                right = false;
            }
            isMoving = false;
        }
        if (collision.collider.tag == "Enemy")
        {
            if (player_state == Player_State.Sword)
            {
                SwordAttack(collision);
                player_state = Player_State.Walk;  // 砍完一次怪就会失去剑
            }
            else Die();
        }

    }

    public void SwordAttack(Collision2D collision)
    {
        //播放砍人动画 
        //TODO
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


