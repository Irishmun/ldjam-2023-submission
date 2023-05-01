using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void ChangeSceneAsync(string name)
    {
        StartCoroutine(LoadScenAsync(name));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    IEnumerator LoadScenAsync(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
