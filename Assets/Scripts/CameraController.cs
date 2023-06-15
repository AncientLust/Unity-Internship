using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerTransform;
    Vector3 startOffset;

    [Range(0, 10)] [SerializeField] float smoothSpeed = 5f;

    void Start()
    {
        startOffset  = transform.position;
        playerTransform = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = playerTransform.position + startOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
