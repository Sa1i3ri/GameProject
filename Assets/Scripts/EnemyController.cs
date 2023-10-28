using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    public static int EnemyNum;

    private void Awake()
    {
        EnemyController.EnemyNum++;
        Debug.Log(EnemyNum);
    }
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
            animator.SetBool("isDead", true);
            // 播放死亡动画
            //animator.SetTrigger("Die");

            // 在动画播放完后销毁敌人对象
            //float deathAnimationLength = GetDeathAnimationLength();
            //Destroy(gameObject, deathAnimationLength);
            transform.GetComponent<BoxCollider2D>().enabled = false;
            EnemyNum--;
            Invoke("Des", 2.0f);
        }
    }

    private void Des()
    {
        Destroy(gameObject);
    }

    // private float GetDeathAnimationLength()
    // {
    //     // 获取死亡动画的长度
    //     if (animator != null)
    //     {
    //         AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
    //         if (clipInfo.Length > 0)
    //         {
    //             return clipInfo[0].clip.length;
    //         }
    //     }
    //     return 0f;
    // }

}