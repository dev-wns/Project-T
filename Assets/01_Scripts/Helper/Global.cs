using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorParameters
{
    public static readonly int IsMove = Animator.StringToHash( "isMove" );
}

static class YieldCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedupdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> Caches = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds( float _seconds )
    {
        WaitForSeconds wfs;
        if ( !Caches.TryGetValue( _seconds, out wfs ) )
        {
            Caches.Add( _seconds, wfs = new WaitForSeconds( _seconds ) );
        }
        return wfs;
    }
}