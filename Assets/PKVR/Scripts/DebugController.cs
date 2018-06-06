using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		if (SystemInfo.supportsGyroscope)
		{
			Input.gyro.enabled = true;
		}
		transform.Translate (Vector3.up * 2.0f);
	}

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
	}
}