using UnityEngine;

public class Pickup : MonoBehaviour
{
    private float rotationSpeedY = 100f; 
    protected ObjectPool _objectPool;
    protected IAudioPlayer _iAudioPlayer;

    public void Init(ObjectPool objectPool, IAudioPlayer iAudioPlayer)
    {
        _objectPool = objectPool;
        _iAudioPlayer = iAudioPlayer;
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float rotationY = rotationSpeedY * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);
    }
}
