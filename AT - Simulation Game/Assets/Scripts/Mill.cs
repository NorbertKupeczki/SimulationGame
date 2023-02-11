using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mill : MonoBehaviour
{
    [SerializeField] private GameObject _blades;
    [SerializeField][Range(0.0f, 80.0f)] private float _rotationSpeed = 0.0f;
    [SerializeField][Range(0.5f, 0.9f)] private float _lerpSpeed = 0.7f;
    [SerializeField] private bool _bladesRotationOn = false;

    private const float _MAX_ROTATION_SPEED = 80.0f;
    private const float _ROTATION_TOLERANCE = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlades();
    }

    public void ToggleMill()
    {
        _bladesRotationOn = !_bladesRotationOn;
    }

    private void UpdateBlades()
    {
        if (_bladesRotationOn && _rotationSpeed < _MAX_ROTATION_SPEED)
        {
            if (Mathf.Abs(_MAX_ROTATION_SPEED - _rotationSpeed) < _ROTATION_TOLERANCE)
            {
                _rotationSpeed = _MAX_ROTATION_SPEED;
            }
            else
            {
                _rotationSpeed = Mathf.Lerp(_rotationSpeed, _MAX_ROTATION_SPEED, _lerpSpeed * Time.deltaTime);            
            }
        }
        else if (!_bladesRotationOn && _rotationSpeed > 0.0f)
        {
            if (_rotationSpeed < _ROTATION_TOLERANCE)
            {
                _rotationSpeed = 0.0f;
            }
            else
            {
                _rotationSpeed = Mathf.Lerp(_rotationSpeed, 0.0f, _lerpSpeed * Time.deltaTime);
            }
        }

        if(_rotationSpeed > 0.0f)
        {
            _blades.transform.Rotate(_blades.transform.forward, _rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
