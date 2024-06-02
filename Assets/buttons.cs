using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
    // Start is called before the first frame update
    public void newGameButton()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("MovementTest");
    }

    public void continueButton()
    {
        SceneManager.LoadSceneAsync("MovementTest");
    }
}
