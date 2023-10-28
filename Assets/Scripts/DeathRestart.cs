using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;

public class DeathRestart : MonoBehaviour
{
    [SerializeField] public static int backTo;

    public static void deathRestart()
    {
        SceneManager.LoadScene(backTo);
    }

    public void toManu()
    {
        SceneManager.LoadScene(0);
    }


}
