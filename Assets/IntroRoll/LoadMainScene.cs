using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour {

    public void LoadScene()
    {
        GUIManagement.CloseAll();
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
