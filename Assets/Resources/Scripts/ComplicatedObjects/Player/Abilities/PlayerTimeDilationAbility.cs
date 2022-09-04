using UnityEngine;

public class PlayerTimeDilationAbility : MonoBehaviour
{
    public KeyCode slowmoKey = KeyCode.T;

    public bool IsTimeSlowed { get; private set; } = false;

    private void Update()
    {
        if (Input.GetKeyDown(slowmoKey) == false) return;
        
        if(IsTimeSlowed == false)
        { 
            Time.timeScale /= 2;
            Time.fixedDeltaTime /= 2;
            IsTimeSlowed = true; 
        }
        else
        { 
            Time.timeScale *= 2;
            Time.fixedDeltaTime *= 2;
            IsTimeSlowed = false; 
        }
    }
}
