using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Object
{
    [SerializeField]
    protected Animator anim { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        Debug.Log( anim );
    }
}
