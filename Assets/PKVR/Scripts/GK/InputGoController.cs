using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InputGoController : MonoBehaviour
{
    public Subject<bool> ovrTouchpad = new Subject<bool> ();

    void Update ()
    {
        var touch = (OVRInput.Get (OVRInput.Touch.PrimaryTouchpad) || OVRInput.Get (OVRInput.Button.PrimaryIndexTrigger)) ? true : false;
        ovrTouchpad.OnNext (touch);
    }
}