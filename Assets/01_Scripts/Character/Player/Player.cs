using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle = 0,
    Run,
}

[System.Serializable]
public class Player : Character
{
    [SerializeField] 
    private Bullet bullet;

    private Slider healthUI;
    private Slider staminaUI;

    private float curSpeed;
    private readonly float walkSpeed = 100.0f;

    private Status stamina = new Status();
    private Status staminaChargingSpeed = new Status();
    private float curStamina = 0.0f;
    private float attackDelay = 0.05f;

    private float invincibleTime = 1.0f;
    private bool isInvincible = false;

    private Vector2 inputXY;

    protected override void Awake()
    {
        base.Awake();

        healthUI = transform.Find( "Health" ).GetComponent<Slider>();
        staminaUI = transform.Find( "Stamina" ).GetComponent<Slider>();

        healthUI.maxValue = 100.0f;
        staminaUI.maxValue = 100.0f;

        stamina.baseValue = 100.0f;
        staminaChargingSpeed.baseValue = 10.0f;
        OnStaminaRecovery();

        StartCoroutine( Attack() );
    }

    private void Update()
    {
        if ( curStamina <= stamina.Value )
        {
            curStamina += staminaChargingSpeed.Value * Time.deltaTime;
        }

        inputXY = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) );
        InverseAxisX( Camera.main.ScreenToWorldPoint( Input.mousePosition ) );

        healthUI.value = curHealth;
        staminaUI.value = curStamina;

        curSpeed = speed.Value;
        if ( Input.GetKey( KeyCode.LeftShift ) )
        {
            curSpeed = walkSpeed;
        }

        if ( Input.GetKeyDown( KeyCode.Space ) && curStamina >= 100.0f )
        {
            curStamina -= 100.0f;
            transform.position = new Vector2( transform.position.x + ( inputXY.x * 100.0f ), transform.position.y + ( inputXY.y * 100.0f ) );
        }

        UpdateAnimatorParameters();
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = inputXY * speed.Value;
    }

    private void OnTriggerEnter2D( Collider2D _col )
    {
        if ( isInvincible )
        {
            return;
        }

        if ( _col.CompareTag( "Enemy" ) )
        {
            StartCoroutine( Invincible() );

            Enemy enemy = _col.gameObject.GetComponent<Enemy>();
            curHealth -= enemy.damage.Value;
        }

        if ( _col.CompareTag( "EnemyBullet" ) )
        {
            Bullet bullet = _col.gameObject.GetComponent<Bullet>();
            if ( ReferenceEquals( bullet, null ) )
            {
                Debug.LogError( "bullet is null." );
                return;
            }

            Enemy enemy = bullet.owner as Enemy;
            if ( ReferenceEquals( bullet, null ) )
            {
                Debug.LogError( "bullet owner is null." );
                return;
            }

            curHealth -= enemy.damage.Value;
            ObjectPool.Instance.Despawn( bullet );
        }
    }

    private void OnStaminaRecovery( int _percent = 100 )
    {
        curStamina += stamina.Value * ( Mathf.Clamp( _percent, 0, 100 ) * 0.01f );

        if ( stamina.Value < curStamina )
        {
            curStamina = stamina.Value;
        }
    }

    private void UpdateAnimatorParameters()
    {
        anim.SetBool( AnimatorParameters.IsMove, rigidbody.velocity.sqrMagnitude > float.Epsilon );
    }

    private IEnumerator Attack()
    {
        while ( true )
        {
            if ( Input.GetMouseButton( 0 ) )
            {
                Vector3 dir = ( Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position ).normalized;
                Vector3 up = Quaternion.Euler( 0.0f, 0.0f, 90.0f ) * dir;
                up.z = 0.0f;

                Vector3 dirXY = new Vector3( dir.x, dir.y, 0.0f );
                Bullet bullet1 = ObjectPool.Instance.Spawn( bullet ) as Bullet;
                Bullet bullet2 = ObjectPool.Instance.Spawn( bullet ) as Bullet;

                bullet1.Initialize( this, transform.position + ( up * 5.0f  ) + ( dirXY * 32.0f ), dirXY );
                bullet2.Initialize( this, transform.position + ( up * -5.0f ) + ( dirXY * 32.0f ), dirXY );
            }

            yield return new WaitForSeconds( attackDelay );
        }
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds( invincibleTime );
        isInvincible = false;
    }
}
