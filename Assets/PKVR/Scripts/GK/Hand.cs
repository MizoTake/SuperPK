using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace PKVR
{
    public class Hand : HandAbstract
    {

        // Use this for initialization
        void Start () => base.HandBind ();
    }
}