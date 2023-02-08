using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = new Vector3(0.0f, -10.0f, 0.0f);
        transform.position = _startPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuilding(GameObject target)
    {
        float topOfObject = target.GetComponent<Collider>().bounds.max.y;
        transform.position = new Vector3(target.transform.position.x, topOfObject, target.transform.position.z);
    }

    public void CancelSelection()
    {
        transform.position = _startPosition;
    }
}
