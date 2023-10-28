using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("to next");
        if (collision.gameObject.tag == "Player" && EnemyController.EnemyNum == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
