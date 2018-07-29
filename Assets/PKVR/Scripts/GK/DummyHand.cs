using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHand : MonoBehaviour
{

    [SerializeField]
    private Hand _hand;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(new Vector3(_hand.transform.position.x * -1f, _hand.transform.position.y, _hand.transform.position.z), Quaternion.Euler(_hand.transform.rotation.x, _hand.transform.rotation.y + 45.0f, _hand.transform.rotation.z));
        Debug.Log(_hand.transform.rotation + " " + transform.rotation);
    }
}
