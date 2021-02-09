using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour
{
    private static Type instance;

    public static Type Instance
    {
        get
        {
            if ( ReferenceEquals( instance, null ) )
            {
                instance = ( Type )FindObjectOfType( typeof( Type ) );

                if ( ReferenceEquals( instance, null ) )
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<Type>();
                    singletonObject.name = typeof( Type ).ToString();

                    DontDestroyOnLoad( singletonObject );
                }
            }
            return instance;
        }
    }
}
