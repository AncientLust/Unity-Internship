using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform _playerTransform;
    Vector3 _startOffset;

    [Range(0, 10)] [SerializeField] float smoothSpeed = 5f;

    void Start()
    {
        _startOffset  = transform.position;
        _playerTransform = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = _playerTransform.position + _startOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
