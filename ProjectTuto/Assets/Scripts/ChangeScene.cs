using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public Dropdown mySelection;

    [SerializeField]
    private int sceneToLoad;

    public static int index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Joueur")
        {
            index = sceneToLoad;
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        }
    }

    public void NextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings > index)
        {
            Controller.score = 0;
            SceneManager.LoadScene(index);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(mySelection.value + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }


}
