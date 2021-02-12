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

    public readonly IReadOnlyCollection<StatusModifier> StatModifiers;
    protected readonly List<StatusModifier> statModifiers;

    public Status()
    {
        statModifiers = new List<StatusModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public Status( float _value ) : this()
    {
        baseValue = _value;
    }

    public virtual void AddModifier( StatusModifier _modifier )
    {
        isDirty = true;
        statModifiers.Add( _modifier );
    }

    public virtual bool RemoveModifier( StatusModifier _modifier )
    {
        if ( statModifiers.Remove( _modifier ) )
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource( object _source )
    {
        int numRemovals = statModifiers.RemoveAll( modifier => modifier.Source == _source );

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

        statModifiers.Sort( CompareModifierOrder );

        for( int count = 0; count < statModifiers.Count; ++count )
        {
            StatusModifier modifier = statModifiers[ count ];

            if ( modifier.Type == StatModType.Flat )
            {
                finalValue += modifier.Value;
            }
            else if ( modifier.Type == StatModType.PercentAdd )
            {
                sumPercentAdd += modifier.Value;

                if ( count + 1 >= statModifiers.Count ||
                     statModifiers[ count + 1 ].Type != StatModType.PercentAdd )
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if ( modifier.Type == StatModType.PercentMult )
            {
                finalValue *= 1 + modifier.Value;
            }
        }

        return ( float )Math.Round( finalValue, 4 );
    }
}
