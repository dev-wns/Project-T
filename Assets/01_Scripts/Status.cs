using System;
using System.Collections.Generic;

[Serializable]
public class Status
{
    public float baseValue;

    protected bool isDirty = true;
    protected float lastBaseValue;

    protected float value;
    public virtual float Value
    {
        get
        {
            if ( isDirty || lastBaseValue != baseValue )
            {
                lastBaseValue = baseValue;
                value = CalcFinalValue();
                isDirty = false;
            }
            return value;
        }
    }

    public readonly IReadOnlyCollection<StatusModifier> StatusModifiers;
    protected readonly List<StatusModifier> statusModifiers;

    public Status()
    {
        statusModifiers = new List<StatusModifier>();
        StatusModifiers = statusModifiers.AsReadOnly();
    }

    public Status( float _value ) : this()
    {
        baseValue = _value;
    }

    public virtual void AddModifier( StatusModifier _modifier )
    {
        isDirty = true;
        statusModifiers.Add( _modifier );
    }

    public virtual bool RemoveModifier( StatusModifier _modifier )
    {
        if ( statusModifiers.Remove( _modifier ) )
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource( object _source )
    {
        int numRemovals = statusModifiers.RemoveAll( modifier => modifier.Source == _source );

        if ( numRemovals > 0 )
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    protected virtual int CompareModifierOrder( StatusModifier _lhs, StatusModifier _rhs )
    {
        if ( _lhs.Order < _rhs.Order)
        {
            return -1;
        }
        else if ( _lhs.Order > _rhs.Order )
        {
            return 1;
        }
        return 0;
    }

    protected virtual float CalcFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0;

        statusModifiers.Sort( CompareModifierOrder );

        for( int count = 0; count < statusModifiers.Count; ++count )
        {
            StatusModifier modifier = statusModifiers[ count ];

            if ( modifier.Type == StatusModifierType.Flat )
            {
                finalValue += modifier.Value;
            }
            else if ( modifier.Type == StatusModifierType.PercentAdd )
            {
                sumPercentAdd += modifier.Value;

                if ( count + 1 >= statusModifiers.Count ||
                     statusModifiers[ count + 1 ].Type != StatusModifierType.PercentAdd )
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if ( modifier.Type == StatusModifierType.PercentMult )
            {
                finalValue *= 1 + modifier.Value;
            }
        }

        return ( float )Math.Round( finalValue, 4 );
    }
}
