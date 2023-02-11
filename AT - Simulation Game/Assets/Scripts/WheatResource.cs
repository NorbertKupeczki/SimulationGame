using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatResource : MonoBehaviour
{
    [Header ("Base data")]
    [SerializeField] private GameObject _resource;
    [SerializeField] [Range (0.0f,100.0f)] private float _growth;
    [SerializeField] WheatState _state;

    [Header("TESTING")]
    [SerializeField] bool _harvest = false;
    [SerializeField] bool _sow = false;

    private const float MAX_GROWTH = 100.0f;
    private const float RIPE_TIME = 60.0f;

    private Vector3 _ripePosition;
    private Vector3 _harvestedPosition;

    private Coroutine _growingWheat_CR;

    public enum WheatState
    {
        HARVESTED = 0,
        GROWING = 1,
        RIPE = 2
    }

    private void Awake()
    {
        Vector3 position = _resource.transform.localPosition;
        _ripePosition = position;
        _harvestedPosition = new Vector3(position.x, -_resource.GetComponent<Renderer>().bounds.extents.y * 2.0f - 0.01f, position.z);

        Harvest();

        _growingWheat_CR = StartCoroutine(GrowingWheat());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case WheatState.HARVESTED:
                {
                    if (_sow)
                    {
                        Debug.Log("Wheat sown");
                        _sow = false;
                        SowWheat();
                    }
                }
                break;
            case WheatState.GROWING:
                {
                    if (_sow || _harvest)
                    {
                        _sow = false;
                        _harvest = false;
                    }
                }
                break;
            case WheatState.RIPE:
                {
                    if (_harvest)
                    {
                        _harvest = false;
                        Debug.Log("Harvested " + Harvest() + " units of wheat");
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(_growingWheat_CR);
    }

    public float Harvest()
    {
        float resourceYield;
        _resource.transform.localPosition = _harvestedPosition;
        _state = WheatState.HARVESTED;
        resourceYield = _growth;
        _growth = 0.0f;
        return resourceYield;
    }

    public void SowWheat()
    {
        _state = WheatState.GROWING;
    }

    private IEnumerator GrowingWheat()
    {
        while(true)
        {
            yield return new WaitUntil(() => _state == WheatState.GROWING);
            while(_growth < MAX_GROWTH)
            {
                float deltaGrowth = MAX_GROWTH / RIPE_TIME * Time.deltaTime;

                if (Mathf.Abs(MAX_GROWTH - _growth) < deltaGrowth)
                {
                    _growth = MAX_GROWTH;
                    _resource.transform.localPosition = _ripePosition;
                    Debug.Log("Wheat is ripe");
                    _state = WheatState.RIPE;
                }
                else
                {
                    _growth += deltaGrowth;
                    _resource.transform.localPosition = Vector3.Lerp(_harvestedPosition, _ripePosition, _growth / MAX_GROWTH);
                }
                yield return null;
            }            
        }
    }
}
