using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    AudioSource audioSource;
    [Header("音效")]
    public AudioClip footStep;
    public AudioClip sword_sound;
    public AudioClip bow_sound;
    public AudioClip bowplus_sound;
    public AudioClip bomb_sound;
    public AudioClip pickCard_sound;

    public static Player Instance;
    [Header("人物设置")]
    public bool isMoving;
    public float speed;
    public float detectDistance;
    public GameObject ArrowPrefab;
    public GameObject ArrowPlusPrefab;
    public LayerMask enemyLayer; // 敌人的层
    public Animator animator;
    [Header("人物贴图")]
    public Sprite normal_idle;
    public Sprite sword_idle;
    public Sprite Usword_idle;
    public Sprite bow_idle;
    public Sprite Ubow_idle;
    public Sprite bomb_idle;


    private bool up;
    private bool down;
    private bool left;
    private bool right;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 currentPosition;

    [SerializeField] int stepNum;
    [SerializeField] Text stepText;

    int deathMenuIndex = 8;
    int noStepIndex = 9;

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
        Bomb,   //摧城
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
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player_state = Player_State.Walk;
        direction = Direction.Left;
        currentPosition = this.transform.position;
        sr.sprite = normal_idle;
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
        Move();
        if (!isMoving) animator.SetBool("isMoving", false);
    }

    private float GetAnimationLength()
    {
        if (animator != null)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                return clipInfo[0].clip.length;
            }
        }
        return 0f;
    }
    void skin(bool needWait)
    {
        StartCoroutine(ChangeStateAndTextureAsync(needWait));
    }
    void setNormalIdle(bool needWait)
    {
        animator.SetBool("SwordOn", false);
        animator.SetBool("BowOn", false);
        animator.SetBool("BombOn", false);
        animator.SetBool("U-SwordOn", false);
        animator.SetBool("U-BowOn", false);
        skin(needWait);
    }
    public void setAnimeOn(string weapon)
    {
        setNormalIdle(false);
        animator.SetBool(weapon, true);
        skin(false);
    }

    private void MoveControl()
    {
        if (isMoving)
            return;
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
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            down = true;
            isMoving = true;
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            left = true;
            isMoving = true;
            direction = Direction.Left;
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            right = true;
            isMoving = true;
            direction = Direction.Right;
            animator.SetBool("isMoving", true);
        }
    }

    private void Move()
    {
        stepText.text = "剩余步数：" + stepNum;
        //人物移动
        if (isMoving == true && this.stepNum > 0)
        {
            audioSource.clip = footStep;
            if (!audioSource.isPlaying)
                audioSource.Play();

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
        if (stepNum <= 0)
        {
            NoStepRestart.backToOrigin = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(this.noStepIndex);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dir = Vector2.zero;
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Enemy"||collision.collider.tag=="EliteEnemy" || collision.collider.tag == "DestroyableWall" || collision.collider.tag=="Door")

        {
            Debug.Log("wall");
            stepNum--;
            if(stepNum <= 0)
            {
                NoStepRestart.backToOrigin = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(this.noStepIndex);
            }
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
            if (collision.collider.tag != "Enemy") isMoving = false;

            if (collision.collider.tag == "Door")
            {
                Debug.Log("Door");

                if (EnemyController.EnemyNum == 0)
                {
                    Debug.Log("to next");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
        if (collision.collider.tag == "DestroyableWall" && player_state == Player_State.Bomb)
        {
            BombAttack(collision);
            return;
        }
        if (collision.collider.tag == "Enemy"|| collision.collider.tag == "EliteEnemy")
        {
            Debug.Log("tag");
            if (player_state == Player_State.Sword)
            {
                Debug.Log("state");
                SwordAttack(collision);
                player_state = Player_State.Walk;  // 砍完一次怪就会失去剑
                setNormalIdle(true);
            }
            else if (player_state == Player_State.SwordPlus)
            {
                SwordAttackPlus(dir);
                player_state = Player_State.Walk;
                setNormalIdle(true);
            }
            else Die();
            isMoving = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "SwordCard")
        {
            Destroy(collision.gameObject);
            SwordCardManager.Instance.PickUpCard();
        }
        else if (collision.tag == "BowCard")
        {
            Destroy(collision.gameObject);
            BowCardManager.Instance.PickUpCard();
        }
        else if (collision.tag == "BombCard")
        {
            Destroy(collision.gameObject);
            BombCardManager.Instance.PickUpCard();
        }
    }

    public void SwordAttack(Collision2D collision)
    {
        animator.SetTrigger("SwordAttack");
        audioSource.clip = sword_sound;
        audioSource.Play();
        Debug.Log("SwordAttack");
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController enemy = collision.collider.GetComponent<EnemyController>();
            enemy.Die();
        }
        else if (collision.gameObject.tag == "EliteEnemy")
        {
            EliteEnemyController eliteEnemy = collision.collider.GetComponent<EliteEnemyController>();
            eliteEnemy.Die();
        }
    }
    public float attackAngle = 45f; // 加强剑攻击的扇形角度（度数不是弧度）
    public float attackDistance = 3f; // 加强剑攻击的距离
    public void SwordAttackPlus(Vector2 attackDirection)
    {
        animator.SetTrigger("U-SwordAttack");
        audioSource.clip = sword_sound;
        audioSource.Play();
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
            else if (hitCollider.CompareTag("EliteEnemy"))
            {
                // 检查是否在扇形范围内
                Vector2 toEnemy = hitCollider.transform.position - transform.position;
                float angleToEnemy = Vector2.Angle(attackDirection, toEnemy);

                if (angleToEnemy <= attackAngle / 2f)
                {
                    // 获取敌人的脚本
                    EliteEnemyController eliteEnemy = hitCollider.GetComponent<EliteEnemyController>();

                    if (eliteEnemy != null)
                    {
                        eliteEnemy.Die();
                    }
                }
            }

        }
        setNormalIdle(true);
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
            audioSource.clip = bow_sound;
            audioSource.Play();
            GameObject arrow = Instantiate(ArrowPrefab, this.transform.position, Quaternion.identity);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            arrowController.Move(dir);
            player_state = Player_State.Walk;
            setNormalIdle(true);
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
            animator.SetTrigger("U-BowAttack");
            audioSource.clip = bowplus_sound;
            audioSource.Play();
            GameObject arrow = Instantiate(ArrowPlusPrefab, this.transform.position, Quaternion.identity);
            ArrowPlusController arrowPlusController = arrow.GetComponent<ArrowPlusController>();
            arrowPlusController.Move(dir);
            player_state = Player_State.Walk;
            setNormalIdle(true);
        }
    }
    private void BombAttack(Collision2D collision)
    {
        animator.SetTrigger("BombAttack");
        audioSource.clip = bomb_sound;
        audioSource.Play();
        DWallController wall = collision.collider.GetComponent<DWallController>();
        wall.Bomb();
        player_state = Player_State.Walk;
        setNormalIdle(true);
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        player_state = Player_State.Dead;
        EnemyController.EnemyNum = 0;

        Invoke("toDeathMenu", 1.5f);

    }

    public void toDeathMenu()
    {
        DeathRestart.backTo = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(this.deathMenuIndex);
    }
    IEnumerator ChangeStateAndTextureAsync(bool needWait)
    {
        if (needWait)
        {
            yield return new WaitForSeconds(0.01f);
            float wait = GetAnimationLength();
            yield return new WaitForSeconds(wait);
        }

        switch (player_state)
        {
            case Player_State.Walk:
                sr.sprite = normal_idle;
                break;
            case Player_State.Sword:
                sr.sprite = sword_idle;
                break;
            case Player_State.SwordPlus:
                sr.sprite = Usword_idle;
                break;
            case Player_State.Bow:
                sr.sprite = bow_idle;
                break;
            case Player_State.BowPlus:
                sr.sprite = Ubow_idle;
                break;
            case Player_State.Bomb:
                sr.sprite = bomb_idle;
                break;
        }
        Debug.Log("sprite updated");
        yield return null;
    }

}