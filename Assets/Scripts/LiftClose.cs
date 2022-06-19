using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftClose : MonoBehaviour
{
    public GameObject [] _tubes;
    public Character _char;
    bool f = false;
    public Material _button;
    
    private void Awake()
    {
        
        _char._act.AddListener(_char_action);
    }

    private void _char_action()
    {
        for (int i = 0; i < _tubes.Length; i++)
        {
            _tubes[i].GetComponentInParent<Animator>().SetBool("Close", !f );
        }
        _button.SetFloat("Speed", 5f);
        Debug.Log(_button.GetFloat("Speed"));
        _char._act.RemoveListener(_char_action);
        f = !f;
    }

    void Close()
    {
        
    }
    
}
