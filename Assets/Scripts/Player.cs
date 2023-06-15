using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10;

    Vector3 _movement;
    Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        transform.position += _movement * _speed * Time.deltaTime;
    }

    void RotatePlayer()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerToMouseDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
