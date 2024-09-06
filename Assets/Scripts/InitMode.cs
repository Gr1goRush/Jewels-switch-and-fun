using UnityEngine;

public class InitMode : MonoBehaviour
{
    SetMode mode;
    [SerializeField] private int Size;

    void Start()
    {
        mode = GetComponent<SetMode>();

        if (mode != null)
        {
            if (Size == 4) mode.Set4();
            else if(Size == 5) mode.Set5();
            else if(Size == 6) mode.Set6();
            else return;
        }
        else
        {
            Debug.LogError("SetMode component not found!");
        }
    }
}
