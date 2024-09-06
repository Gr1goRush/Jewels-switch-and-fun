using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.iOS;

public class ButtonsController : MonoBehaviour
{
    public void Rate()
    {
        Device.RequestStoreReview();
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RestartLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
