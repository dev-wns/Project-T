using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<Type> : MonoBehaviour where Type : MonoBehaviour
{
    private Type prefab;
    private Queue<Type> waitingPool = new Queue<Type>();
    private int capacity = 100;
    private int spare = 10;


    private void Initialize()
    {
        if ( ReferenceEquals( prefab, null ) )
        {
            Debug.LogError( "Prefab is null during object pool initalization." );
            return;
        }

        for ( int count = 0; count < capacity; ++count )
        {
            Type type = Instantiate( prefab );
            type.gameObject.SetActive( false );
            waitingPool.Enqueue( type );
        }
    }

    public Type Spawn()
    {
        if ( waitingPool.Count <= spare )
        {
            Initialize();
        }

        Type type = waitingPool.Dequeue();
        type.gameObject.SetActive( true );
        return type;
    }

    public Type Spawn( GameObject _parent )
    {
        if ( waitingPool.Count <= spare )
        {
            Initialize();
        }

        Type type = waitingPool.Dequeue();
        type.gameObject.SetActive( true );
        type.gameObject.transform.SetParent( _parent.transform );
        return type;
    }

    public Type Spawn( Vector3 _pos, GameObject _parent )
    {
        if ( waitingPool.Count <= spare )
        {
            Initialize();
        }

        Type type = waitingPool.Dequeue();
        type.gameObject.SetActive( true );
        type.transform.position = _pos;
        type.gameObject.transform.SetParent( _parent.transform );
        return type;
    }

    public Type Spawn( Vector3 _pos )
    {
        if ( waitingPool.Count <= spare )
        {
            Initialize();
        }

        Type type = waitingPool.Dequeue();
        type.gameObject.SetActive( true );
        type.transform.position = _pos;
        return type;
    }

    public Type Spawn( Vector3 _pos, Quaternion _rot )
    {
        if ( waitingPool.Count <= spare )
        {
            Initialize();
        }

        Type type = waitingPool.Dequeue();
        type.gameObject.SetActive( true );
        type.transform.position = _pos;
        type.transform.rotation = _rot;
        return type;
    }

    public void Despawn( Type _type )
    {
        _type.gameObject.SetActive( false );
        waitingPool.Enqueue( _type );
    }
}
