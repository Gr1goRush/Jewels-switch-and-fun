using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Slider slider;
    public string sceneName;
    public float loadingTime = 5f;

    private void Start()
    {
        StartCoroutine(LoadSceneAsyncWithTimer());
    }

    IEnumerator LoadSceneAsyncWithTimer()
    {
        float timer = 0f;

        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            float progress = timer / loadingTime;
            slider.value = progress;
            yield return null; 
        }

        yield return StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
