using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer { get; private set; }

    protected Vector3 position { get; private set; }

    protected Quaternion rotation { get; private set; }

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        position = transform.position;
        rotation = transform.rotation;
    }
}
