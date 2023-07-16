using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _playerTransform;
    private float smoothSpeed = 5f;
    private Vector3 _startOffset = new Vector3(0, 10, 0);

    public void Init(Transform playerTarget)
    {
        _playerTransform = playerTarget;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (_playerTransform != null)
        {
            var targetPosition = _playerTransform.position + _startOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
        }
    }

    public void MoveToPlayer()
    {
        transform.position = _playerTransform.position;
    }
}
