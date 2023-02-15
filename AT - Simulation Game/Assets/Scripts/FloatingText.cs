using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] float _speed = 20.0f;
    [SerializeField] float _lifeTime = 3.0f;
    [SerializeField] float _startingY = 0.0f;
    [SerializeField] string _name = "Floating Text";

    private RectTransform _rectTransform;
    private Vector3 _moveVector;

    private void Awake()
    {
        name = "[Floating text] " + _name;
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.localPosition = new Vector3(0.0f, _startingY, 0.0f);
        _moveVector = new Vector3(0.0f, -_speed, 0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_lifeTime > 0.0f)
        {
            _rectTransform.localPosition -= _moveVector * Time.deltaTime;
            _lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
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
