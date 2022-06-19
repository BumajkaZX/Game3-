using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class LptMove : MonoBehaviour
{
    Vector3 _endPoint;
    public Vector3 _startPoint;
    public float step;
    float progress;
    public float shift;
    bool forward = true;
    
    void Start()
    {
        _startPoint = transform.localPosition;
        _endPoint = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + shift);
        Observable.EveryFixedUpdate().Subscribe(_ =>
        {
            Debug.Log(progress);
            if (forward)
            {
                transform.localPosition = Vector3.Lerp(_startPoint, _endPoint, progress);
                progress += step;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(_endPoint, _startPoint, progress);
                progress += step;
            }
            
        }).AddTo(this);
        Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(_ =>
        {
            forward = !forward;
            progress = 0;

        }).AddTo(this);
    }
     
    void Update()
    {
        
    }
}
