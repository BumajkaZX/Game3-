using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

public class Character : MonoBehaviour
{   public  UnityEvent _act = new UnityEvent();
    public float _speed;
    public float _shiftSpeed;
    Vector3 _dir;
    Rigidbody _rb;
    Animator _anim;
    SpriteRenderer _sp;
    public bool invertY;
    public bool invertX;
    float invY = 1;
    float invX = 1;
    public bool invertSprite;
    Animator _clip;
    public float _clipspeed;
    public Collider _trigger;
    public GameObject _UIButton;
    public GameObject _UIButtonPar;
    

    public CompositeDisposable _dis = new CompositeDisposable();
    public CompositeDisposable _disUse = new CompositeDisposable();


   

    void Start()
    {
        _clip = GetComponent<Animator>();
        _sp = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(freeze());
        CharAlive();
    }
    void CharAlive()
    {
        
      
        if (invertY)
        {
            invY = -1;
        } 
        if (invertX)
        {
            invX = -1; 
        }
       
        Observable.EveryFixedUpdate().Subscribe(_ =>
           {
               _dir = new Vector3(Input.GetAxis("Horizontal") * invX , 0, Input.GetAxis("Vertical") * invY);
               OnkeyDown(_dir);
           }).AddTo(this);
        Observable.EveryUpdate().Where(_ => Input.GetButtonDown("SpeedBoost")).Subscribe(_ =>
        {
            _speed += _shiftSpeed;
            _clip.SetFloat("Speed", 1f + _clipspeed);
        }).AddTo(_dis);
        Observable.EveryUpdate().Where(_ => Input.GetButtonUp("SpeedBoost")).Subscribe(_ =>
        {
            _speed -= _shiftSpeed;
            _clip.SetFloat("Speed", 1f );
        }).AddTo(_dis);
        _trigger.OnTriggerEnterAsObservable().Select(t => t.gameObject)
            .Subscribe(t =>
        {
            _UIButtonPar.transform.position = new Vector3(t.transform.position.x, t.transform.position.y + 0.1f, t.transform.position.z);
            _UIButton.SetActive(true);
            Observable.EveryUpdate().Where(_ => Input.GetButtonDown("Use")).Subscribe(_ => 
            {
                _act.Invoke();
                _UIButton.SetActive(false);
                Destroy(t);
            }).AddTo(_disUse);
        }).AddTo(_dis);
        _trigger.OnTriggerExitAsObservable().Subscribe(_ => 
        {
            _disUse.Clear();
            _UIButton.SetActive(false);
        }
        ).AddTo(_dis)
        ;
        
        }
    
    
   

    void OnkeyDown(Vector3 _d) 
    {
        if (_d.x != 0 || _d.z != 0)
        {
            _anim.SetBool("Run", true);
        }
        else { _anim.SetBool("Run", false); }
        if (_d.x < 0)
        {
            _sp.flipX = !invertSprite;
        }
        else if (_d.x > 0) { _sp.flipX = invertSprite; }
    _rb.MovePosition(transform.position + (_d * _speed / 1000));
    }
    IEnumerator freeze()
    {
        
        yield return new WaitForSeconds(0.5f);
        _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
   public void DisClear()
    {
        _dis.Clear();
    }
}
