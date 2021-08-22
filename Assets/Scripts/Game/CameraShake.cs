using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float shakeFrequency = 0.2f;
    [SerializeField] private float _secondsCameraShakes = 0.6f;

    private bool _canShakeCamera;
    private float _duration;
    private Vector3 _originalPosOfCam;

    void Start()
    {
        _originalPosOfCam = _cameraTransform.position;
    }

    void Update()
    {
        if(_canShakeCamera == true && _duration > Time.time)
        {
            ShakeCamera();
        }
        else if(Time.time > _duration && _canShakeCamera == true)
        {
            _canShakeCamera = false;
        }
    }

    private void ShakeCamera()
    {
        _cameraTransform.transform.position = _originalPosOfCam + Random.insideUnitSphere * shakeFrequency;
    }
    
    public void StartShakingCamera()
    {
        _duration = Time.time + _secondsCameraShakes;
        _canShakeCamera = true;

    }
}
