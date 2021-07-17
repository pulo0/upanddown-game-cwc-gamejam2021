using UnityEngine;

public class OrbMovement : MonoBehaviour
{
    //Components
    private Transform _orbTransform;
    
    //Floats
    private const float Frequency = 0.5f;
    private const float Amplitude = 0.2f;
    
    //Vector
    private Vector2 _posOffset;
    private Vector2 _tempPos;

    private void Awake()
    {
        _orbTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        _posOffset = _orbTransform.position;
    }

    private void Update()
    {
        _tempPos = _posOffset;
        _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Amplitude;

        _orbTransform.position = _tempPos;
    }
    
}
