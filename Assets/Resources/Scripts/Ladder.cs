using UnityEngine;

public class Ladder : MonoBehaviour
{
    public float upwardsSpeed = 1;
    public float downwardsSpeed = 1.5f;

    protected KeyCode _upKey;
    protected KeyCode _downKey;
    protected Rigidbody _playerRB;

    private PlayerMovement _player;
    private bool _isBeingClimbed = false;


    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<PlayerMovement>();
        if (_player == null) return;

        _isBeingClimbed = true;

        _upKey = _player.climbUpwardsKey;
        _downKey = _player.climbDownwardsKey;

        _playerRB = _player.GetComponent<Rigidbody>();
        _playerRB.useGravity = false;
    }

    private void OnTriggerExit(Collider other)
    {
        var playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement == null) return;

        _isBeingClimbed = false;

        _playerRB.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (_isBeingClimbed == false || _player == null) return;

        Climb();
    }

    protected virtual void Climb()
    {
        if (Input.GetKey(_upKey))
            _player.transform.position += gameObject.transform.up * upwardsSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(_downKey))
            _player.transform.position += gameObject.transform.up * -downwardsSpeed * Time.fixedDeltaTime;
    }
}
