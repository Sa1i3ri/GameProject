using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameJump : MonoBehaviour
{

    [SerializeField] public int level_1;
    [SerializeField] public int level_2;
    [SerializeField] public int level_3;
    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToLevel_1()
    {
        SceneManager.LoadScene(level_1);
    }

    public void ToLevel_2()
    {
        SceneManager.LoadScene(level_2);
    }

    public void ToLevel_3()
    {
        SceneManager.LoadScene(level_3);
    }



}
