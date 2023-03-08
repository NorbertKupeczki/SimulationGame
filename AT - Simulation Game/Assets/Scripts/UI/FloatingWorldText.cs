using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingWorldText : MonoBehaviour
{
    private Vector3 _position;
    private Camera _camera;
    private TextMeshProUGUI _text;

    private const float DURATION = 2.0f;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void InitText(Camera camera, Vector3 position, string message)
    {
        _camera = camera;
        _position = position + Vector3.up;
        _text.text = message;
        transform.position = _camera.WorldToScreenPoint(_position);

        StartCoroutine(FloatText(DURATION));
    }

    private IEnumerator FloatText(float timer)
    {
        float cd = timer;
        while (cd > 0)
        {
            yield return null;
            cd -= Time.deltaTime;

            _position += Vector3.up * Time.deltaTime;
            transform.position = _camera.WorldToScreenPoint(_position);
        }

        Destroy(gameObject);
    }
}
