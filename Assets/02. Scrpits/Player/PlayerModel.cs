using Info;
using Player.Item;
using Player.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Skill;
using System.Reflection;

namespace Player
{
	public class PlayerModel : MonoBehaviour
	{
		#region define

		[Header("Stat")]
		[SerializeField]
		protected PlayerComponentStatSO		_cpnStatSO;

		[Header("Attack")]
		[SerializeField]
		protected PlayerBullet[]			_bulletPrefabs;
		[SerializeField]
		protected Transform					_bulletSummonTr;
		[SerializeField]
		protected float						_attackCooltime = 0.2f;

		[Header("Skill")]
		[SerializeField]
		protected APlayerSkillDataSO[]		_skillDataSO;
		[SerializeField]
		protected PoolableMono[]			_summonablePrefabs;

		[SerializeField]
		protected Inventory					inventory;

		public Quaternion					cameraRotation;
		public Vector3						moveDirection;

		protected Transform					bulletParent;
		protected PlayerComponentSkill		cpnSkill;
		protected PlayerComponentBuff		cpnBuff;
		protected PlayerComponentStat		cpnStat;
		protected APlayerComponentAnimation	cpnAnimation;
		protected Rigidbody					rigid;
		protected PlayerPool				pool;
		protected WaitForSeconds			attackCooldown;
		protected IRaycastable				raycaster;

		protected bool						canAttack = true;
		protected bool						isGrounded = true;
		protected bool						canDamaged = true;
		protected bool						isWaitDamaged = false;
		protected Coroutine					waitDamagedCoroutine;

		public PlayerComponentSkill			Skill { get => cpnSkill; }
		public PlayerComponentBuff			Buff { get => cpnBuff; }
		public PlayerComponentStat			Stat { get => cpnStat; }
		public APlayerComponentAnimation	Animation { get => cpnAnimation; }
		public Inventory					Inventory { get => inventory; }
		public PlayerPool					Pool { get => pool; }

		public bool							CanDamaged { get => (canDamaged || isWaitDamaged); }

		public bool							IsAlive { get; protected set; } = true;
		public bool							IsMoveable { get; protected set; } = true;

		public event Action<PlayerModel>	ActionCallbackBuffChanged;
		public event Action<PlayerModel>	ActionCallbackStatChanged;
		public event Action<PlayerModel>	ActionCallbackItemChanged;
		public event Action<PlayerModel>	ActionCallbackLanded;

		public delegate void InfoIntHandler(ref SInfoInt info);
		public delegate void InfoAttackHandler(ref SInfoAttack info);
		public delegate void InfoBuffHandler(ref SInfoBuff info);

		public event InfoIntHandler			ActionOnBeforeAddShield;
		public event InfoIntHandler			ActionOnBeforeRemoveShield;
		public event InfoIntHandler			ActionOnBeforeHeal;
		public event InfoAttackHandler		ActionOnBeforeDamage;
		public event InfoAttackHandler		ActionOnBeforeDeal;

		public event Action<SInfoInt>		ActionOnAfterAddShield;
		public event Action<SInfoInt>		ActionOnAfterRemoveShield;
		public event Action<SInfoInt>		ActionOnAfterHeal;
		public event Action<SInfoAttack>	ActionOnAfterDamage;
		public event Action<SInfoAttack>	ActionOnAfterDeal;

		#endregion

		#region UnityEvent

		protected virtual void Awake()
		{
			int i = 1;

			rigid = GetComponentInParent<Rigidbody>();
			bulletParent = new GameObject("PlayerBulletParent").transform;
			cpnSkill = new PlayerComponentSkill(this, _skillDataSO);
			cpnBuff = new PlayerComponentBuff(this);
			cpnStat = new PlayerComponentStat(this, _cpnStatSO);
			pool = new PlayerPool(bulletParent);
			inventory = new Inventory(this);
			attackCooldown = new WaitForSeconds(_attackCooltime);
			pool.CreatePool(_bulletPrefabs[0], 40);
			while (i < _bulletPrefabs.Length)
			{
				pool.CreatePool(_bulletPrefabs[i++], 10);
			}
			i = 0;
			while (i < _summonablePrefabs.Length)
			{
				pool.CreatePool(_summonablePrefabs[i++], 3);
			}
			IsMoveable = true;
			return ;
		}

		protected virtual void Update()
		{
			if (cpnBuff.UpdateBuff(Time.deltaTime))
			{
				ActionCallbackBuffChanged?.Invoke(this);
			}
			cpnSkill.UpdateSkill(Time.deltaTime);
			inventory.UpdateItem(Time.deltaTime);
			cpnAnimation.Update(Time.deltaTime);
			return ;
		}

		protected virtual void OnDestroy()
		{
			if (bulletParent != null)
				Destroy(bulletParent.gameObject);
			return ;
		}

		public void SetRaycaster(IRaycastable raycastable)
		{
			raycaster = raycastable;
			return ;
		}

		#endregion

		#region Move & Jump & Turn

		public virtual Vector3 Move(Transform parent, Vector3 movement, bool isSprint, Action callback = null)
		{
			float speed;

			if (!IsMoveable)
				return (Vector3.zero);
			speed = Stat.GetSpeed(isSprint);
			parent.position += movement * speed;
			moveDirection = movement.normalized;
			callback?.Invoke();
			return (movement * speed);
		}

		public bool Jump(Action callback = null)
		{
			return (Jump(Stat.GetJumpPower(), callback));
		}

		public virtual bool Jump(float jumpForce, Action callback = null)
		{
			if (isGrounded)
			{
				rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				isGrounded = false;
				Stat.CountJump();
				callback?.Invoke();
				return (true);
			}
			else if (Stat.CanJump())
			{
				rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				Stat.CountJump();
				callback?.Invoke();
				return (true);
			}
			return (false);
		}

		public void Dash(Action callback = null)
		{
			Dash(Stat.Stat.powerDash, moveDirection, callback);
			return;
		}

		public virtual void Dash(float power, Vector3 movement, Action callback = null)
		{
			if (movement.sqrMagnitude > 0)
				rigid.velocity += power * movement.normalized;
			else
				rigid.velocity += power * transform.forward;
			callback?.Invoke();
			return ;
		}

		public virtual void OnGround()
		{
			Stat.ResetJumpCount();
			isGrounded = true;
			ActionCallbackLanded?.Invoke(this);
			return;
		}

		public Quaternion Rotate(Transform parent, float x)
		{
			parent.Rotate(Vector3.up, x, Space.World);
			return (transform.rotation);
		}

		public void SetCameraRotation(Quaternion rotation)
		{
			cameraRotation = rotation;
			return ;
		}

		#endregion

		#region Attack

		public virtual bool Attack(Vector3 targetPos)
		{
			if (canAttack)
			{
				canAttack = false;
				Shoot(targetPos, 0,30f, 0.02f);
				StartCoroutine(WaitAttack());
				return (true);
			}
			return (false);
		}

		public void Shoot(int index = 0, float speed = 5f, float spread = 0.04f)
		{
			Vector2 targetPos;

			if (raycaster == null)
				throw (new Exception("Cannot Found Raycaster in PlayerModel!"));
			targetPos = raycaster.GetRaycastHitPoint();
			Shoot(targetPos, index, speed, spread);
			return ;
		}

		public void Shoot(string name, float speed = 5f, float spread = 0.04f)
		{
			Vector3 targetPos;

			if (raycaster == null)
				throw (new Exception("Cannot Found Raycaster in PlayerModel!"));
			targetPos = raycaster.GetRaycastHitPoint();
			Shoot(targetPos, name, speed, spread);
			return ;
		}

		public void Shoot(Vector3 targetPos, int index = 0, float speed = 5f, float spread = 0.04f)
		{
			if (index >= _bulletPrefabs.Length)
			{
				index = _bulletPrefabs.Length - 1;
				Debug.LogError($"Cannot found bullet {index}");
			}
			Shoot(targetPos, _bulletPrefabs[index].gameObject.name, speed, spread);
			return ;
		}

		public virtual void Shoot(Vector3 targetPos, string name, float speed = 5f, float spread = 0.04f)
		{
			PlayerBullet bullet;
			Vector3 direction = GetSpreadDirection((targetPos - _bulletSummonTr.position).normalized, spread);

			try
			{
				bullet = (pool.Pop(name) as PlayerBullet);
			}
			catch (Exception e)
			{
				bullet = (pool.Pop(_bulletPrefabs[0].gameObject.name) as PlayerBullet);
				Debug.LogError($"[PlayerModel_Shoot_Pool]\n{e.Message}");
			}
			bullet.SetInfo(this);
			bullet.transform.position = _bulletSummonTr.position;
			bullet.transform.LookAt(_bulletSummonTr.position + direction);
			bullet.SetSpeed(speed);
			return ;
		}

		public Vector3 GetSpreadDirection(Vector3 forward, float spread = 0.04f)
		{
			Vector3 random = UnityEngine.Random.insideUnitSphere * spread;
			Vector3 direction = (forward + random).normalized;

			return (direction);
		}

		private IEnumerator WaitAttack()
		{
			yield return (attackCooldown);
			canAttack = true;
			yield break ;
		}

		public void Summon(int index = 0, float speed = 5f, float spread = 0.04f)
		{
			Vector2 targetPos;

			if (raycaster == null)
				throw (new Exception("Cannot Found Raycaster in PlayerModel!"));
			targetPos = raycaster.GetRaycastHitPoint();
			Summon(targetPos, index, speed, spread);
			return;
		}

		public void Summon(string name, float speed = 5f, float spread = 0.04f)
		{
			Vector3 targetPos;

			if (raycaster == null)
				throw (new Exception("Cannot Found Raycaster in PlayerModel!"));
			targetPos = raycaster.GetRaycastHitPoint();
			Summon(targetPos, name, speed, spread);
			return;
		}

		public void Summon(Vector3 targetPos, int index = 0, float speed = 5f, float spread = 0.04f)
		{
			if (index >=_summonablePrefabs.Length)
			{
				index = _summonablePrefabs.Length - 1;
				Debug.LogError($"Cannot found bullet {index}");
			}
			Summon(targetPos, _summonablePrefabs[index].gameObject.name, speed, spread);
			return;
		}

		// TODO!
		public virtual void Summon(Vector3 targetPos, string name, float speed = 5f, float spread = 0.04f)
		{
			PoolableMono target;
			Vector3 direction = GetSpreadDirection((targetPos - _bulletSummonTr.position).normalized, spread);

			try
			{
				target = (pool.Pop(name));
			}
			catch (Exception e)
			{
				target = pool.Pop(_summonablePrefabs[0].gameObject.name);
				Debug.LogError($"[PlayerModel_Shoot_Pool]\n{e.Message}");
			}
			target.transform.position = _bulletSummonTr.position;
			target.transform.LookAt(_bulletSummonTr.position + direction);
			return;
		}

		#endregion

		#region Stat

		public virtual int AddShield(SInfoInt info)
		{
			int result;

			ActionOnBeforeAddShield?.Invoke(ref info);
			result = AddShield(info.value);
			ActionOnAfterAddShield?.Invoke(info);
			return (result);
		}

		protected int AddShield(int add)
		{
			int result = Stat.AddShield(add);

			ActionCallbackStatChanged?.Invoke(this);
			return (result);
		}

		public virtual int RemoveShield(SInfoInt info)
		{
			int result;

			ActionOnBeforeRemoveShield?.Invoke(ref info);
			result = RemoveShield(info.value);
			ActionOnAfterRemoveShield?.Invoke(info);
			return (result);
		}

		protected int RemoveShield(int remove)
		{
			int result = Stat.RemoveShield(remove);

			ActionCallbackStatChanged?.Invoke(this);
			return (result);
		}

		public virtual int Healed(SInfoInt info)
		{
			int result;

			ActionOnBeforeHeal?.Invoke(ref info);
			result = Healed(info.value);
			ActionOnAfterHeal?.Invoke(info);
			return (result);
		}

		protected int Healed(int heal)
		{
			int result = Stat.Healed(heal);

			ActionCallbackStatChanged?.Invoke(this);
			return (result);
		}

		public int Damaged(SInfoAttack info)
		{
			return (Damaged(info, false, 1f));
		}

		public int Damaged(SInfoAttack info, bool isIgnoreWaitDamaged)
		{
			return (Damaged(info, isIgnoreWaitDamaged, 1f));
		}

		public int Damaged(SInfoAttack info, float waitDamagedTime)
		{
			return (Damaged(info, false, waitDamagedTime));
		}

		public virtual int Damaged(SInfoAttack info, bool isIgnoreWaitDamaged, float waitDamagedTime)
		{
			int result;

			if (!isIgnoreWaitDamaged && isWaitDamaged)
				return (-1);
			if (isIgnoreWaitDamaged && waitDamagedCoroutine != null && waitDamagedTime > 0f)
				StopCoroutine(waitDamagedCoroutine);
			if (waitDamagedTime > 0f)
				waitDamagedCoroutine = StartCoroutine(WaitDamaged(waitDamagedTime));
			ActionOnBeforeDamage?.Invoke(ref info);
			result = Damaged(info.damage);
			ActionOnAfterDamage?.Invoke(info);
			return (result);
		}

		protected int Damaged(int damage)
		{
			int result = Stat.Damaged(damage);

			IsAlive = Stat.IsAlive();
			ActionCallbackStatChanged?.Invoke(this);
			return (result);
		}

		protected virtual IEnumerator WaitDamaged(float time)
		{
			isWaitDamaged = true;
			yield return (new WaitForSeconds(time));
			isWaitDamaged = false;
			yield break ;
		}

		// TODO!
		protected virtual int Deal(GameObject target, int damage, ElementType type = null)
		{
			SInfoAttack info = new(gameObject, target, damage, type);

			ActionOnBeforeDeal?.Invoke(ref info);
			// 데미지 가하기 처리
			ActionOnAfterDeal(info);
			return (info.damage);
		}

		#endregion

		#region Buff

		// TODO!
		public virtual void AddBuff(SInfoBuff info)
		{
			cpnBuff.AddBuff(info);
			ActionCallbackBuffChanged?.Invoke(this);
			return ;
		}

		#endregion

		#region Skill

		public virtual bool UseSkill(short index, KeyCode skillKey)
		{
			Debug.Log($"PlayerModel : Skill {index} use input");
			return (cpnSkill.UseSkill(index, skillKey));
		}

		#endregion

		#region Item

		public void AddItem(AItem item)
		{
			inventory.AddItem(item);
			ActionCallbackItemChanged?.Invoke(this);
			return ;
		}

		public void RemovevItem(AItem item)
		{
			if (inventory.RemoveItem(item))
				ActionCallbackItemChanged?.Invoke(this);
			return ;
		}

		#endregion
	}
}