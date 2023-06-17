using UnityEngine;

public class Projectile : MonoBehaviour
{
    float _speed = 20;

    void Update()
    {
        MoveForward();
        DestroyOnceOutOfSight();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    void DestroyOnceOutOfSight()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(transform.gameObject);
        }
    }
}
