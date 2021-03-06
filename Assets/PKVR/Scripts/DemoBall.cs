﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRandom = UnityEngine.Random;
using System.Threading.Tasks;
using DG.Tweening;

public class DemoBall : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private float power = 10.0f;

    // Use this for initialization
    void Start()
    {
        var initPos = transform.position;
        var initRot = transform.rotation;

        Observable
            .Timer(TimeSpan.FromSeconds(5))
            .Do(_ =>
           {
               transform.position = initPos;
               transform.rotation = initRot;
               rigid.velocity = Vector3.zero;
           })
            .Repeat()
            .Subscribe(_ =>
           {
               var randRotY = UniRandom.Range(-18f, 18f);
               DOTween.Sequence()
                   .AppendInterval(0.5f)
                   .Append(transform.DORotate(Vector3.up * randRotY, 0.0f, RotateMode.LocalAxisAdd))
                   .OnComplete(() => rigid.AddForce(Vector3.back * power, ForceMode.Impulse))
                   .Play();
           })
            .AddTo(this);
    }
}