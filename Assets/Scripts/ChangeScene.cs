using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Load1()
    {
        SceneManager.LoadScene(1);
    }

    public void Load2()
    {
        SceneManager.LoadScene(2);
    }

    public void Load3()
    {
        SceneManager.LoadScene(3);
    }
}
