//using UnityEngine;

//public class PlayerCombat : PirateMonoBehaviour
//{
//    public GameObject cannonballPrefab;
//    public Transform firePoint;
//    [SerializeField] protected float cannonballSpeed = 20f;

//    [SerializeField] protected float fireCooldown = 1.5f;
//    protected float lastFireTime = -999f;
//    public float LastFireTime => lastFireTime;

//    protected Rigidbody rb;
//    protected ShipStateBase currentState;

//    protected virtual void Update()
//    {
//        if (InputManager.Instance.IsAttacking())
//        {
//            FireCannon();

//            if (!(currentState is ShipCombatState))
//            {
//                ChangeState(new ShipCombatState(this));
//            }
//        }

//        currentState?.Update();
//    }
//}
