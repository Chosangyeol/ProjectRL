using Info;
using Player.Item;
using Player.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Skill;
using System.Security.Cryptography;
using System.Diagnostics.Tracing;
using System.Xml.Serialization;

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
		protected PoolableMono				_bulletPrefab;
		[SerializeField]
		protected Transform					_bulletSummonTr;
		[SerializeField]
		protected float						_attackCooltime = 0.2f;

		[Header("Skill")]
		[SerializeField]
		protected APlayerSkillDataSO[]		_skillDataSO;

		[SerializeField]
		protected Inventory					inventory;

		public Vector3						angleCamera;

		protected Transform					bulletParent;
		protected PlayerComponentSkill		cpnSkill;
		protected PlayerComponentBuff		cpnBuff;
		protected PlayerComponentStat		cpnStat;
		protected Rigidbody					rigid;
		protected PlayerPool				pool;
		protected WaitForSeconds			attackCooldown;
		protected bool						canAttack = true;
		protected bool						isGrounded = true;

		public PlayerComponentSkill			Skill { get => cpnSkill; }
		public PlayerComponentBuff			Buff { get => cpnBuff; }
		public PlayerComponentStat			Stat { get => cpnStat; }
		public Inventory					Inventory { get => inventory; }
		public PlayerPool					Pool { get => pool; }

		public bool							IsAlive { get; private set; }
		public bool							IsMoveable { get; private set; }

		public event Action					ActionCallbackBuffChanged;
		public event Action					ActionCallbackStatChanged;
		public event Action					ActionCallbackItemChanged;
		public event Action					ActionCallbackLanded;

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
			rigid = GetComponentInParent<Rigidbody>();
			bulletParent = new GameObject("PlayerBulletParent").transform;
			cpnSkill = new PlayerComponentSkill(this, _skillDataSO);
			cpnBuff = new PlayerComponentBuff(this);
			cpnStat = new PlayerComponentStat(this, _cpnStatSO);
			pool = new PlayerPool(bulletParent);
			inventory = new Inventory(this);
			attackCooldown = new WaitForSeconds(_attackCooltime);
			pool.CreatePool(_bulletPrefab, 40);
			IsMoveable = true;
			return ;
		}

		protected virtual void Update()
		{
			if (cpnBuff.UpdateBuff(Time.deltaTime))
			{
				ActionCallbackBuffChanged?.Invoke();
			}
			cpnSkill.UpdateSkill(Time.deltaTime);
			inventory.UpdateItem(Time.deltaTime);
			return ;
		}

		protected virtual void OnDestroy()
		{
			if (bulletParent != null)
				Destroy(bulletParent.gameObject);
			return ;
		}

		#endregion

		#region Move & Jump & Turn

		public virtual float Move(Transform parent, Vector3 movement, bool isSprint, Action callback = null)
		{
			float speed;

			if (!IsMoveable)
				return (0);
			speed = Stat.GetSpeed(isSprint);
			parent.position += movement * speed;
			callback?.Invoke();
			return (movement.sqrMagnitude * speed);
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

		public void OnGround()
		{
			Stat.ResetJumpCount();
			isGrounded = true;
			ActionCallbackLanded?.Invoke();
			return;
		}

		public Quaternion Rotate(Transform parent, float x)
		{
			parent.Rotate(Vector3.up, x, Space.World);
			return (transform.rotation);
		}

		public void SetAngle(Vector3 angle)
		{
			angleCamera = angle;
			return ;
		}

		#endregion

		#region Attack

		public virtual void Attack()
		{
			if (canAttack)
			{
				canAttack = false;
				Shoot();
				StartCoroutine(WaitAttack());
			}
			return ;
		}

		public virtual void Shoot(float speed = 5f)
		{
			PlayerBullet bullet = pool.Pop(_bulletPrefab.gameObject.name).GetComponent<PlayerBullet>();

			bullet.SetInfo(this);
			bullet.transform.position = _bulletSummonTr.position;
			bullet.transform.rotation = Quaternion.Euler(angleCamera);
			bullet.SetSpeed(speed);
			return ;
		}

		private IEnumerator WaitAttack()
		{
			yield return (attackCooldown);
			canAttack = true;
			yield break ;
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

			ActionCallbackStatChanged?.Invoke();
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

			ActionCallbackStatChanged?.Invoke();
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

			ActionCallbackStatChanged?.Invoke();
			return (result);
		}

		public virtual int Damaged(SInfoAttack info)
		{
			int result;

			ActionOnBeforeDamage?.Invoke(ref info);
			result = Damaged(info.damage);
			ActionOnAfterDamage?.Invoke(info);
			return (result);
		}

		protected int Damaged(int damage)
		{
			int result = Stat.Damaged(damage);

			IsAlive = Stat.IsAlive();
			ActionCallbackStatChanged?.Invoke();
			return (result);
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
			ActionCallbackBuffChanged?.Invoke();
			return ;
		}

		#endregion

		#region Skill

		public virtual bool UseSkill(short index)
		{
			return (cpnSkill.UseSkill(index));
		}

		#endregion

		#region Item

		public void AddItem(AItem item)
		{
			inventory.AddItem(item);
			ActionCallbackItemChanged?.Invoke();
			return ;
		}

		public void RemovevItem(AItem item)
		{
			if (inventory.RemoveItem(item))
				ActionCallbackItemChanged?.Invoke();
			return ;
		}

		#endregion
	}
}