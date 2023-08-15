using UnityEngine;

public class Pickup : MonoBehaviour
{
    private float _lifetime = 10f;
    private float _timeElapsed = 0f;
    private float _rotationSpeedY = 100f; 
    protected ObjectPool _objectPool;
    protected IAudioPlayer _iAudioPlayer;

    private Vector3 originalScale;

    public void Init(ObjectPool objectPool, IAudioPlayer iAudioPlayer)
    {
        _objectPool = objectPool;
        _iAudioPlayer = iAudioPlayer;
    }

    private void OnEnable()
    {
        _timeElapsed = 0f;
        originalScale = transform.localScale;
    }

    void Update()
    {
        Rotate();
        ReduceScale();
        LifeTimeCheck();
    }

    void ReduceScale()
    {
        _timeElapsed += Time.deltaTime;
        float scaleRatio = 1 - (_timeElapsed / _lifetime);
        transform.localScale = originalScale * scaleRatio;
    }

    private void LifeTimeCheck()
    {
        if (_timeElapsed >= _lifetime)
        {
            _objectPool.Return(gameObject);
            return;
        }
    }

    private void Rotate()
    {
        float rotationY = _rotationSpeedY * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);
    }
}
