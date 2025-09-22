using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerModel : MonoBehaviour
	{
		[Header("Components")]
		private PlayerComponentSkill cpnSkill;
		[SerializeField]
		private PlayerComponentBuff cpnBuff;
		[SerializeField]
		private PlayerComponentStat cpnStat;

		private Rigidbody rigid;
		private bool isGrounded = true;
		private Vector3 angleCamera;

		public bool IsAlive { get; private set; }
		public bool IsMoveable { get; private set; }

		public event Action ActionCallbackSkillChanged;
		public event Action ActionCallbackBuffChanged;
		public event Action ActionCallbackStatChanged;
		public event Action ActionCallbackLanding;

		void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			cpnSkill = new PlayerComponentSkill(this);
			cpnBuff = new PlayerComponentBuff(this);
			cpnStat = new PlayerComponentStat();
			cpnStat.speedMove = 3f;
			cpnStat.speedSprint = 7f;
			IsMoveable = true;
			return;
		}

		public float Move(Transform parent, Vector3 movement, bool isSprint, Action callback = null)
		{
			float speed;

			if (!IsMoveable)
				return (0);
			if (isSprint)
			{
				speed = cpnStat.speedSprint;
			}
			else
			{
				speed = cpnStat.speedMove;
			}
				parent.position += movement * speed;
			callback?.Invoke();
			return (movement.sqrMagnitude);
		}

		public bool Jump(Action callback = null)
		{
			return (Jump(cpnStat.jumpPower, callback));
		}

		public bool Jump(float jumpForce, Action callback = null)
		{
			if (isGrounded)
			{
				rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				isGrounded = false;
				cpnStat.CountJump();
				callback?.Invoke();
				return (true);
			}
			else if (cpnStat.CanJump())
			{
				rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				cpnStat.CountJump();
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
				cpnStat.ResetJumpCount();
				isGrounded = true;
				ActionCallbackLanding?.Invoke();
			}
			return;
		}
	}
}