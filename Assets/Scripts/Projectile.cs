using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed = 20;
    [SerializeField] public float Damage { get; set; } = 0;
    
    void Update()
    {
        if (!GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            return;
        }

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
            gameObject.SetActive(false);
        }
    }
}
