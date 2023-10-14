using CCN.Collections;
using MenuManagement.Base;
using UnityEngine;
using UnityEngine.Events;

namespace CCN.Health
{
    [GroupEvents("onDamage", "onHeal", "onDeath", "onSpawnOrRespawn")]
    [GroupProperties("Sound", "", "audioSource", "playOnDamage", "playOnHeal", "playOnDeath")]
    [GroupProperties("Connected Objects", "", "activateWhenInvincible", "spawnOnDeath", "destroyOnDeath")]
    [GroupProperties("Setup", "", "health", "timeInvincibleAfterSpawn", "autoRespawnDelay", "_RigidBody", "hitBoxes", "shields")]
    public class DamageReceiver : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField, Tooltip("How much damage this object has")]
        protected float health = 100f;
        
        [SerializeField, Tooltip("Duration for which the object will be invincible after spawn")]
        private float timeInvincibleAfterSpawn = 0f;

        [SerializeField, Tooltip("Shields to protect this object")]
        private ShieldCollection shields;

        [SerializeField, Tooltip("Time interval after which this object will respawn. value <= 0 means no respawning")]
        public float autoRespawnDelay;

        [SerializeField] private Rigidbody _RigidBody;

        [Tooltip("Colliders that take damage (You can have different parts of object receive damage differently.")]
        public HitBoxCollection hitBoxes;

        [Tooltip("Visuals for the invisible")]
        public GameObject activateWhenInvincible;

        [Tooltip("Objects to spawn when the object dies")]
        public GameObjectCollection spawnOnDeath;

        [Tooltip("Objects to destroy when the object dies")]
        public GameObjectCollection destroyOnDeath;


        [Tooltip("Audio source to play clips")]
        public AudioSource audioSource;

        [Tooltip("One of these clips will be played when the this object receives damage ")]
        public AudioClipsCollection playOnDamage;

        [Tooltip("One of these clips will be played when the this object dies ")]
        public AudioClipsCollection playOnHeal;

        [Tooltip("One of these clips will be played when the this object dies ")]
        public AudioClipsCollection playOnDeath;

        [SerializeField, Tooltip("Event called when this object is damaged")]
        private UnityEvent<DamageInfo> onDamage;

        [SerializeField, Tooltip("Event called when this object is healed")]
        private UnityEvent<float, Healer> onHeal;

        [SerializeField, Tooltip("Event called when this object dies")]
        private UnityEvent onDeath;

        [SerializeField, Tooltip("Event called when this object is spawned or respawned")]
        private UnityEvent<bool> onSpawnOrRespawn;

        private bool _isInvincible = false;
        
        protected float startValue;

        public float LastSpawnTime { get; protected set; }

        /// <summary> How much damage this object has </summary>
        public float Health => health;

        /// <summary> Nothing can damage this object while its invincible </summary>
        public bool IsInvincible
        {
            get { return _isInvincible || timeInvincibleAfterSpawn + LastSpawnTime < Time.time; }
            set
            {
                _isInvincible = value;
                if (activateWhenInvincible) activateWhenInvincible.SetActive(value);
            }
        }
        #endregion

        #region Editor Functions
        private void Reset()
        {
            _RigidBody = GetComponent<Rigidbody>();


            hitBoxes = new HitBoxCollection();
            foreach (Collider child in GetComponentsInChildren<Collider>())
            {
                hitBoxes.Add(new HitBox(child) { name = child.name });
            }
        }
        #endregion

        #region Event subs/unsubs
        /// <summary> Subscribe to onDamage Event of this object </summary>
        public void AddOnDamageListener(UnityAction<DamageInfo> call) => onDamage.AddListener(call);

        /// <summary> Subscribe to onHeal Event of this object </summary>
        public void AddOnHealListener(UnityAction<float, Healer> call) => onHeal.AddListener(call);

        /// <summary> Subscribe to onDeath Event of this object </summary>
        public void AddOnDeathListener(UnityAction call) => onDeath.AddListener(call);

        /// <summary> Subscribe to onSpawnOrRespawn Event of this object </summary>
        public void AddOnSpawnOrRespawnListener(UnityAction<bool> call) => onSpawnOrRespawn.AddListener(call);

        /// <summary> Unsubscribe from onDamage Event of this object </summary>
        public void RemoveOnDamageListener(UnityAction<DamageInfo> call) => onDamage.RemoveListener(call);

        /// <summary> Unsubscribe from onHeal Event of this object </summary>
        public void RemoveOnHealListener(UnityAction<float, Healer> call) => onHeal.RemoveListener(call);

        /// <summary> Unsubscribe from onDeath Event of this object </summary>
        public void RemoveOnDeathListener(UnityAction call) => onDeath.RemoveListener(call);

        /// <summary> Unsubscribe from onSpawnOrRespawn Event of this object </summary>
        public void RemoveOnSpawnOrRespawnListener(UnityAction<bool> call) => onSpawnOrRespawn.AddListener(call);
        #endregion

        #region Functionalities
        protected virtual void Start()
        {
            startValue = health;
            shields.Init();
            OnSpawnOrRespawn(false);
            onSpawnOrRespawn?.Invoke(false);
        }

        
        public void AddShield(float life, float absorptionRate, bool restoreOnRespawn = true)
        {
            AddShield(new Shield(life, absorptionRate, restoreOnRespawn));
        }
        
        public void AddShield(Shield shield)
        {
            shield.Init();
            shields.Add(shield);
        }

        public void RemoveShield(Shield shield)
        {
            shields.Remove(shield);
        }

        /// <summary> Damage this object </summary>
        public void Damage(DamageInfo damage)
        {
            if (CanDamage() == false) return;

            if (damage.hitBox == null && HitBoxCollection.TryGet(hitBoxes, damage, out HitBox hitBox))
            {
                damage = new DamageInfo(damage.rawAmount, damage.dealer, damage.position, damage.force, hitBox.Collider, hitBox);
            }

            OnDamage(damage);
            onDamage?.Invoke(damage);
        }

        /// <summary> Heal this object </summary>
        /// <param name="amount"> How much health to heal </param>
        /// <param name="healer"> The healer that healed this object </param>
        public void Heal(float amount, Healer healer)
        {
            OnHeal(amount, healer);
            onHeal?.Invoke(amount, healer);
        }

        /// <summary> Instantly kill this object. </summary>
        public void Die()
        {
            OnDeath();
            onDeath?.Invoke();
            if (autoRespawnDelay == 0) Respawn();
            else if (autoRespawnDelay > 0) Invoke(nameof(Respawn), autoRespawnDelay);
        }

        /// <summary> Instantly respawn this object </summary>
        public void Respawn()
        {
            OnSpawnOrRespawn(true);
            onSpawnOrRespawn?.Invoke(true);
        }

        /// <returns> Is the object alive </returns>
        public virtual bool IsAlive()
        {
            return health > 0;
        }

        /// <returns> Can the object take damage </returns>
        public virtual bool CanDamage()
        {
            return IsAlive() == true && IsInvincible == false;
        }
        #endregion

        #region Callbacks
        /// <summary> Callback for when the object receives damage </summary>
        protected virtual void OnDamage(DamageInfo damage)
        {
            if (shields != null && shields.Count > 0)
            {
                damage.Amount = shields.Damage(damage.Amount);
            }

            health -= damage.Amount;

            if (damage.hitBox != null && damage.hitBox.damageMarks != null) damage.hitBox.damageMarks.SpawnRandom(damage.position, Quaternion.identity, transform);
            if (_RigidBody != null) _RigidBody.AddForceAtPosition(damage.force, damage.position);

            if (IsAlive() == false) OnDeath();
            else if (audioSource != null && playOnDamage != null) playOnDamage.TryPlayRandom(audioSource);
        }

        /// <summary> Callback for when the object is healed </summary>
        /// <param name="amount"> How much health to heal </param>
        /// <param name="healer"> The healer that healed this object </param>
        protected virtual void OnHeal(float amount, Healer healer)
        {
            health += amount;
            if (audioSource != null && playOnHeal != null) playOnHeal.TryPlayRandom(audioSource);
        }

        /// <summary> Callback for when the object is killed </summary>
        protected virtual void OnDeath()
        {
            health = 0;
            if (spawnOnDeath != null && spawnOnDeath.Count > 0)
            {
                Transform mTrans = transform;
                spawnOnDeath.SpawnSelf(mTrans.position, mTrans.rotation, mTrans);
            }

            if (destroyOnDeath != null && destroyOnDeath.Count > 0) destroyOnDeath.DestroySelf();
            if (audioSource != null && playOnDeath != null) playOnDeath.TryPlayRandom(audioSource);
        }

        /// <summary> Callback for when the object is spawned/respawned </summary>
        protected virtual void OnSpawnOrRespawn(bool isRespawning)
        {
            LastSpawnTime = Time.time;
            health = startValue;

            if (!isRespawning) return;
            shields.OnRespawn();
        }
        #endregion
    }
}