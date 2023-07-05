using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _playerTarget;
    private float smoothSpeed = 5f;
    private Vector3 _startOffset = new Vector3(0, 10, 0);

    public void Init(Transform playerTarget)
    {
        _playerTarget = playerTarget;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (_playerTarget != null)
        {
            var targetPosition = _playerTarget.position + _startOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
        }
    }
}
