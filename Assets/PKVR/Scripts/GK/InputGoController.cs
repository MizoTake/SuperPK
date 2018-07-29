using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputGoController : MonoBehaviour
{

    public Subject<float> ovrTouchpad = new Subject<float>();

    void Update()
    {
        var touchValue = (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) ? 0.3f : -0.3f;
        ovrTouchpad.OnNext(touchValue);
    }
}
