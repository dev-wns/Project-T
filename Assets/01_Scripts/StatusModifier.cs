
public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

public class StatusModifier 
{
    public readonly float Value;
    public readonly StatModType Type;
    public readonly int Order;
    public readonly object Source;

    public StatusModifier( float _value, StatModType _type, int _order, object _source )
    {
        Value = _value;
        Type = _type;
        Order = _order;
        Source = _source;
    }

    public StatusModifier( float _value, StatModType _type ) : this( _value, _type, (int)_type, null ) { }

    public StatusModifier( float _value, StatModType _type, int _order ) : this( _value, _type, _order, null ) { }

    public StatusModifier( float _value, StatModType _type, object _source ) : this( _value, _type, (int)_type, _source ) { }
}
