using UnityEngine;

public class SetMode : MonoBehaviour
{
    [Header("Containers")]
    [SerializeField] private GameObject mode;
    [SerializeField] private GameObject main;

    [Header("Fields")]
    [SerializeField] private GameObject[] items4;
    [SerializeField] private GameObject[] items5;
    [SerializeField] private GameObject[] items6;

    public void SetGameMode(GameObject[] items)
    {
        foreach (GameObject obj in items)
        {
            obj.SetActive(false);
        }
        mode.SetActive(false);
        main.SetActive(true);
    }

    public void Set4()
    {
        SetGameMode(items4);
    }

    public void Set5()
    {
        SetGameMode(items5);
    }

    public void Set6()
    {
        SetGameMode(items6);
    }
}
