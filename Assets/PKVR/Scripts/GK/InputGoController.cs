using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InputGoController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 0.3f;

    public Subject<float> ovrTouchpad = new Subject<float> ();

    void Update ()
    {
        var touchValue = (OVRInput.Get (OVRInput.Touch.PrimaryTouchpad) || OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger)) ? _speed : -_speed;
        ovrTouchpad.OnNext (touchValue);
    }
}