using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using RanRange = UnityEngine.Random;
using System.Threading.Tasks;
using DG.Tweening;

public class DemoBall : MonoBehaviour
{

	[SerializeField]
	private Rigidbody rigid;
	[SerializeField]
	private float power = 10.0f;

	// Use this for initialization
	void Start ()
	{
		var initPos = transform.position + Vector3.up;
		var initRot = transform.rotation;

		Observable
			.Timer (TimeSpan.FromSeconds (3))
			.Do (_ =>
			{
				transform.position = initPos;
				transform.rotation = initRot;
				rigid.velocity = Vector3.zero;
			})
			.Repeat ()
			.Subscribe (async _ =>
			{
				await Shoot ();
			})
			.AddTo (this);
	}

	private async Task Shoot ()
	{
		var randRotY = RanRange.Range (-50f, 50f);
		transform.DORotate (Vector3.up * randRotY, 0.0f, RotateMode.LocalAxisAdd).Play ();
		rigid.AddForce (transform.forward * power, ForceMode.Impulse);
		await Task.Delay (TimeSpan.FromSeconds (3.0f));
	}
}