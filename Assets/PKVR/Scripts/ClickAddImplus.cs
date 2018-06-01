using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAddImplus : MonoBehaviour
{

	[SerializeField]
	private Rigidbody _rigid;
	[SerializeField]
	private float _power;

	// Use this for initialization
	void Start ()
	{
		_rigid.AddForce (transform.forward * _power, ForceMode.Impulse);
	}

	// Update is called once per frame
	void Update ()
	{

	}
}