using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UniRx;

public class CamDepth : MonoBehaviour
{
    Vector3 _offset;
    public GameObject _point;
    public GameObject _pers;
    public CinemachineVirtualCamera _cam;
    float _defOffset;
    CinemachineTransposer v;
    public float delOffset;

    public CompositeDisposable _dis = new CompositeDisposable();
    void Start()
    {
       v  = _cam.GetCinemachineComponent<CinemachineTransposer>();
        _offset = v.m_FollowOffset;
        _defOffset = _offset.z;
        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (_pers.transform.position.z > _point.transform.position.z)
            {
                v.m_FollowOffset = new Vector3(_offset.x, _offset.y, _defOffset + ((_pers.transform.position.z - _point.transform.position.z) / _defOffset));
            }
        }).AddTo(_dis);
    }

    public void DisClear()
    {
        _dis.Clear();
    }
 
}
