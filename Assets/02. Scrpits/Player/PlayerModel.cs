using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerModel : MonoBehaviour
	{
		[Header("Movement")]
		[SerializeField]
		private float _moveSpeed = 7.0f;
		[SerializeField]
		private float _jumpForce = 5.0f;

		private Rigidbody rigid;
		private bool isGrounded = true;

		public SPlayerStat PlayerState { get; private set; }
		public bool IsAlive { get; private set; }
		public bool IsMoveable { get; private set; }

		public event Action ActionCallbackValueChanged;
		public event Action ActionCallbackLanding;

		void Awake()
		{
			rigid = GetComponent<Rigidbody>();
			return;
		}

		public float Move(Vector3 movement, Action callback = null)
		{
			Vector3 result;

			if (!IsMoveable)
				return (0);
			result = _moveSpeed * movement;
			transform.position += result;
			callback?.Invoke();
			return (result.sqrMagnitude);
		}

		public bool Jump(Action callback = null)
		{
			return (Jump(_jumpForce, callback));
		}

		public bool Jump(float jumpForce, Action callback = null)
		{
			if (isGrounded)
			{
				rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				isGrounded = false;
				callback?.Invoke();
				return (true);
			}
			return (false);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("Ground"))
			{
				isGrounded = true;
			}
			ActionCallbackLanding?.Invoke();
			return;
		}
	}
}