using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DWallController : MonoBehaviour
{
    public LayerMask enemyLayer; // 敌人的层
    public float explosionRadius;// 爆炸半径
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Bomb()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyController enemy = hitCollider.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.Die();
                }
            }
            if (hitCollider.CompareTag("EliteEnemy"))
            {
                EliteEnemyController eliteEnemy = hitCollider.GetComponent<EliteEnemyController>();

                if (eliteEnemy != null)
                {
                    eliteEnemy.Dead();
                }
            }

        }

        // 插入坍塌动画
        transform.GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("Bomb");
        Destroy(gameObject, 0.5f);
    }
}
