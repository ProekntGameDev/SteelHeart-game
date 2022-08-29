using System.Collections;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    public float time = 2;


    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovementController>();
        if (player != null) StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (time > 0)
        {
            time -= Time.fixedDeltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
