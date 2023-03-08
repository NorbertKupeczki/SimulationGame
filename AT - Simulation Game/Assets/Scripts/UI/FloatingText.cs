using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float _speed = 20.0f;
    [SerializeField] float _lifeTime = 3.0f;
    [SerializeField] [Range(0.01f, 1.0f)] float _fadeRate = 1.0f;
    [SerializeField] float _startingY = 0.0f;

    private RectTransform _rectTransform;
    private Vector3 _moveVector;
    private TextMeshProUGUI _text;
    private float _timer;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<TextMeshProUGUI>();

        _rectTransform.localPosition = new Vector3(0.0f, _startingY, 0.0f);
        _moveVector = new Vector3(0.0f, -_speed, 0.0f);
    }

    void Update()
    {
        _rectTransform.localPosition -= _moveVector * Time.deltaTime;
        _timer += Time.deltaTime;

        if(_lifeTime < _timer)
        {
            double fade = 0.02f/((0.8 * (_timer - _lifeTime) * Time.deltaTime * -_fadeRate) - 0.02f) + 1;
            _text.alpha = Mathf.Lerp(_text.alpha, 0.0f, (float)fade);
        }

        if (_text.alpha < 0.05f)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
        name = "[Floating text] " + text;
    }

    public void SetStartingPosition(float x, float y)
    {
        _rectTransform.localPosition = new Vector3(x, y, 0.0f);
    }

    public void SetStartingPosition(Vector3 position)
    {
        _rectTransform.localPosition = position;
    }
}
