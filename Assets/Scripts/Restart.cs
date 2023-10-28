using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void restart()
    {
        EnemyController.EnemyNum = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
