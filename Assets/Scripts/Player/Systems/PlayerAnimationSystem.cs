using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Vector3 _directionVetor;

    public void Init(Rigidbody rigidbody)
    {
        _animator = GetComponent<Animator>();
        _rigidbody = rigidbody;
    }

    private void Update()
    {

        _directionVetor.x = Input.GetAxisRaw("Horizontal");
        _directionVetor.z = Input.GetAxisRaw("Vertical");

        if (_directionVetor.x != 0 || _directionVetor.z != 0)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _animator.SetTrigger("deathTrigger");
        }


        if (Input.GetKeyDown(KeyCode.X))
        {
            _animator.ResetTrigger("deathTrigger");
        }
    }
}
