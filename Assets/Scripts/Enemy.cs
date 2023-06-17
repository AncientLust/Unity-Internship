using UnityEngine;

public class Enemy : MonoBehaviour
{
    float _speed = 5f;
    Transform _playerTransform;

    void Start()
    {
        _playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        transform.LookAt(_playerTransform);
    }
}
