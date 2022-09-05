using UnityEngine;

public class Climbable : Ladder
{
    protected override void Climb()
    {
        base.Climb();
        if (Input.GetKey(_upKey) == false && Input.GetKey(_downKey) == false)
        {
            _playerRB.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }
}
