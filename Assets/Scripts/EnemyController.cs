using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            // 播放死亡动画
            animator.SetTrigger("Die");

            // 在动画播放完后销毁敌人对象
            float deathAnimationLength = GetDeathAnimationLength();
            Destroy(gameObject, deathAnimationLength);
        }
    }

    private float GetDeathAnimationLength()
    {
        // 获取死亡动画的长度
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
}
