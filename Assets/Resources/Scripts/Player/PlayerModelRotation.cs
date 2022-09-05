using UnityEngine;

public class PlayerModelRotation : MonoBehaviour
{
    private Vector3 v3_left_rotation = new Vector3(0, 180, 0);
    private Vector3 v3_right_rotation = new Vector3(0, 0, 0);

    private PlayerShootingAbility _shootingAbility;


    private void Awake()
    {
        _shootingAbility = GetComponent<PlayerShootingAbility>();
    }

    private void Update()
    {
        bool isShooting = false;
        if (_shootingAbility != null) isShooting = _shootingAbility.IsShooting;

        if (isShooting)
        {
            Vector3 cursor_delta = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            if (cursor_delta.x < 0) transform.eulerAngles = v3_left_rotation;
            else if (cursor_delta.x > 0) transform.eulerAngles = v3_right_rotation;
        }
        else
        {
            float MoveControl_HorizontalAxis = Input.GetAxis("Horizontal");
            bool isMoveLeft = MoveControl_HorizontalAxis < 0;
            bool isMoveRight = MoveControl_HorizontalAxis > 0;
            if (isMoveLeft) transform.eulerAngles = v3_left_rotation;
            else if (isMoveRight) transform.eulerAngles = v3_right_rotation;
        }
        
    }
}
