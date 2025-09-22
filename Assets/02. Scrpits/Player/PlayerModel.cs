using Player.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerModel : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField]
		private PlayerComponentStatSO _cpnStatSO;

		[Header("Inventory")]
		[SerializeField]
		private Inventory inventory;

		private PlayerComponentSkill cpnSkill;
		private PlayerComponentBuff cpnBuff;
		private PlayerComponentStat cpnStat;
		private Rigidbody rigid;
		private bool isGrounded = true;
		private Vector3 angleCamera;

		public PlayerComponentSkill Skill { get => cpnSkill; }
		public PlayerComponentBuff Buff { get => cpnBuff; }
		public PlayerComponentStat Stat { get => cpnStat; }
		public Inventory Inventory { get => inventory; }

		public bool IsAlive { get; private set; }
		public bool IsMoveable { get; private set; }

		public event Action ActionCallbackSkillChanged;
		public event Action ActionCallbackBuffChanged;
		public event Action ActionCallbackStatChanged;
		public event Action ActionCallbackLanding;

		#region Init

		void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			cpnSkill = new PlayerComponentSkill(this);
			cpnBuff = new PlayerComponentBuff(this);
			cpnStat = new PlayerComponentStat(_cpnStatSO);
			inventory = new Inventory();
			IsMoveable = true;
			return;
		}

		#endregion

		#region Move & Jump &Turn

		public float Move(Transform parent, Vector3 movement, bool isSprint, Action callback = null)
		{
			float speed;

			if (!IsMoveable)
				return (0);
			if (isSprint)
			{
				speed = _cpnStatSO.speedSprint;
			}
			else
			{
				speed = _cpnStatSO.speedMove;
			}
				parent.position += movement * speed;
			callback?.Invoke();
			return (movement.sqrMagnitude);
		}

		public bool Jump(Action callback = null)
		{
			return (Jump(_cpnStatSO.jumpPower, callback));
		}

		public bool Jump(float jumpForce, Action callback = null)
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

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("Ground"))
			{
				Stat.ResetJumpCount();
				isGrounded = true;
				ActionCallbackLanding?.Invoke();
			}
			return;
		}

		#endregion

		#region Stat

		public int AddShield(int add)
		{
			int result = Stat.AddShield(add);

			ActionCallbackStatChanged?.Invoke();
			return (result);
		}

		public int RemoveShield(int remove)
		{
			int result = Stat.RemoveShield(remove);

			ActionCallbackStatChanged?.Invoke();
			return (result);
		}

		public int Healed(int heal)
		{
			int result = Stat.Healed(heal);

			ActionCallbackStatChanged?.Invoke();
			return (result);
		}

		// TODO!
		public int Damaged(AttackInfo info)
		{
			throw (new NotImplementedException());
		}

		public int Damaged(int damage)
		{
			int result = Stat.Damaged(damage);

			IsAlive = Stat.IsAlive();
			ActionCallbackStatChanged?.Invoke();
			return (result);
		}

		#endregion
	}
}