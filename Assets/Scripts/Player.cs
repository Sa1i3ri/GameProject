using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public float speed;
    public float detectDistance;
    public GameObject ArrowPrefab;
    public GameObject ArrowPlusPrefab;
    public LayerMask enemyLayer; // 敌人的层
    public Animator animator;

    private bool isMoving;
    private bool up;
    private bool down;
    private bool left;
    private bool right;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 currentPosition;

    [SerializeField] int stepNum;
    [SerializeField] Text stepText;

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
        SwordPlus,
        Bow,
        BowPlus,
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
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player_state = Player_State.Walk;
        direction = Direction.Left;
        currentPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_state == Player_State.Dead) return;
        MoveControl();
    }

    private void FixedUpdate()
    {
        if (player_state == Player_State.Dead) return;
        if (isMoving) animator.SetBool("isMoving", true);
        Move();
        if (!isMoving) animator.SetBool("isMoving", false);
    }
    void setNormalIdle()
    {
        animator.SetBool("SwordOn", false);
        animator.SetBool("BowOn", false);
        animator.SetBool("BombOn", false);
        animator.SetBool("U-SwordOn", false);
        animator.SetBool("U-Bow", false);
    }
    public void setAnimeOn(string weapon)
    {
        setNormalIdle();
        animator.SetBool(weapon, true);
    }

    private void MoveControl()
    {
        if (isMoving)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player_state = Player_State.Walk;
            setNormalIdle();
        }
        if (player_state == Player_State.Bow)
        {//射箭时不会移动
            BowAttack();
            return;
        }
        else if (player_state == Player_State.BowPlus)
        {
            BowAttackPlus();
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
        stepText.text = "剩余步数：" + stepNum;
        //人物移动
        if (isMoving == true)
        {
            if (stepNum <= 0)
            {
                return;
            }
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
                stepNum--;
                isMoving = false;
                right = false;
                this.transform.position = currentPosition;
            }
        }
        else if (left == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.left, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                stepNum--;
                isMoving = false;
                left = false;
                this.transform.position = currentPosition;
            }
        }
        else if (up == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.up, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                stepNum--;
                isMoving = false;
                up = false;
                this.transform.position = currentPosition;
            }
        }
        else if (down == true)
        {
            if (Physics2D.Raycast(rb.position, Vector2.down, detectDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                stepNum--;
                isMoving = false;
                down = false;
                this.transform.position = currentPosition;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dir = Vector2.zero;
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Enemy" || collision.collider.tag == "DestroyableWall")
        {
            stepNum--;
            if (up == true)
            {
                dir = new Vector2(0, 1);
                this.transform.position = currentPosition;
                up = false;
            }
            if (down == true)
            {
                dir = new Vector2(0, -1);
                this.transform.position = currentPosition;
                down = false;
            }
            if (left == true)
            {
                dir = new Vector2(-1, 0);
                this.transform.position = currentPosition;
                left = false;
            }
            if (right == true)
            {
                dir = new Vector2(1, 0);
                this.transform.position = currentPosition;
                right = false;
            }
            isMoving = false;
        }
        if (collision.collider.tag == "DestroyableWall" && player_state == Player_State.Bomb)
        {
            BombAttack(collision);
            return;
        }
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("tag");
            if (player_state == Player_State.Sword)
            {
                Debug.Log("state");
                SwordAttack(collision);
                player_state = Player_State.Walk;  // 砍完一次怪就会失去剑
                setNormalIdle();
            }
            else if (player_state == Player_State.SwordPlus)
            {
                SwordAttackPlus(dir);
                player_state = Player_State.Walk;
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
        animator.SetTrigger("SwordAttack");
        Debug.Log("SwordAttack");
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        enemy.Die();
    }
    public float attackAngle = 45f; // 加强剑攻击的扇形角度（度数不是弧度）
    public float attackDistance = 3f; // 加强剑攻击的距离
    public void SwordAttackPlus(Vector2 attackDirection)
    {
        animator.SetTrigger("SwordAttack");
        // 检测在扇形范围内的敌人
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackDistance, enemyLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // 检查是否在扇形范围内
                Vector2 toEnemy = hitCollider.transform.position - transform.position;
                float angleToEnemy = Vector2.Angle(attackDirection, toEnemy);

                if (angleToEnemy <= attackAngle / 2f)
                {
                    // 获取敌人的脚本
                    EnemyController enemy = hitCollider.GetComponent<EnemyController>();

                    if (enemy != null)
                    {
                        enemy.Die();
                    }
                }
            }

        }
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
            animator.SetTrigger("BowAttack");
            GameObject arrow = Instantiate(ArrowPrefab, this.transform.position, Quaternion.identity);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            arrowController.Move(dir);
            player_state = Player_State.Walk;
            setNormalIdle();
        }


    }
    public void BowAttackPlus()
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
            //animator.SetTrigger("BowAttackPlus");
            GameObject arrow = Instantiate(ArrowPlusPrefab, this.transform.position, Quaternion.identity);
            ArrowPlusController arrowPlusController = arrow.GetComponent<ArrowPlusController>();
            arrowPlusController.Move(dir);
            player_state = Player_State.Walk;
        }
    }
    private void BombAttack(Collision2D collision)
    {
        animator.SetTrigger("BombAttack");
        DWallController wall = collision.collider.GetComponent<DWallController>();
        wall.Bomb();
        player_state = Player_State.Walk;
        setNormalIdle();
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        player_state = Player_State.Dead;
    }
}


