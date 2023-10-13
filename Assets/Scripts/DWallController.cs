using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DWallController : MonoBehaviour
{
    public float explosionRadius;// 爆炸半径
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Bomb()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

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
        }

        // 插入坍塌动画
        //TODO
        Destroy(gameObject);
    }
}
