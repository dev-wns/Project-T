using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField]
    private GameObject bulletCanvas;
    [SerializeField]
    private List<Object> prefabs = new List<Object>();

    private Dictionary<string /* prefab name */, Queue<Object>> waitingPool = new Dictionary<string, Queue<Object>>();
    private int capacity = 100;

    private void Initialize( Object _prefab )
    {
        string keyName = _prefab.name;
        if ( !waitingPool.ContainsKey( keyName ) )
        {
            Debug.LogError( _prefab.name + " : Prefab is null during object pool initalization." );
            return;
        }

        Transform organizedTransform = bulletCanvas.transform.Find( keyName );
        if ( ReferenceEquals( organizedTransform, null ) )
        {
            GameObject organizedObject = new GameObject();
            organizedObject.name = keyName;
            organizedObject.transform.SetParent( bulletCanvas.transform );
            organizedTransform = organizedObject.transform;
        }

        for ( int count = 0; count < capacity; ++count )
        {
            Object obj = Instantiate( _prefab );
            obj.name = keyName;
            obj.gameObject.SetActive( false );
            obj.transform.SetParent( organizedTransform );
            waitingPool[ keyName ].Enqueue( obj );
        }
    }

    public void Despawn( Object _object )
    {
        string keyName = _object.name;
        if ( ReferenceEquals( _object, null ) )
        {
            Debug.LogError( "object is null." );
            return;
        }

        _object.gameObject.SetActive( false );
        waitingPool[ keyName ].Enqueue( _object );
    }

    public Object Spawn( Object _prefab )
    {
        string keyName = _prefab.name;
        if ( !waitingPool.ContainsKey( keyName ) )
        {
            waitingPool.Add( keyName, new Queue<Object>() );
        }

        if ( waitingPool[ keyName ].Count == 0 )
        {
            Initialize( _prefab );
        }

        Object obj = waitingPool[ keyName ].Dequeue();
        obj.gameObject.SetActive( true );
        return obj;
    }
}
