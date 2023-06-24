using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Transform _playerTarget;
    private Vector3 _startOffset;

    private void Start()
    {
        _startOffset  = transform.position;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        var targetPosition = _playerTarget.position + _startOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);
    }
}
