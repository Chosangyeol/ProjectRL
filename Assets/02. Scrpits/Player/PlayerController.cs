using Config.UserInput;
using System;
using UnityEngine;
using UnitySubCore.Resolve;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("Player Object")]
		[SerializeField]
		private PlayerModel _playerModel;
		[SerializeField]
		private PlayerCamera _playerCamera;

		[Header("Input Mouse")]
		[SerializeField]
		private float _mouseRX;
		[SerializeField]
		private float _mouseRY;

		private bool isFixedCursor = true;

		public PlayerModel Player { get => _playerModel; }
		public PlayerCamera Camera { get => _playerCamera; }

		public event Action ActionCallbackMove;
		public event Action ActionCallbackJump;
		public event Action ActionCallbackTurn;

		private void Awake()
		{
			return ;
		}

		void Update()
		{
			Move();
			if (Input.GetKeyDown(ConfigUserInput.Instance.keyJump))
				Jump();
			if (isFixedCursor)
				Turn();
			return ;
		}

		public void FixCursor(bool toggle)
		{
			isFixedCursor = toggle;
			if (isFixedCursor)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
			}
			return ;
		}

		private void Move()
		{
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");
			Vector3 moveCamF = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1));
			Vector3 moveCamR = Vector3.Scale(Camera.transform.right, new Vector3(1, 0, 1));
			Vector3 movement = moveCamF.normalized * moveVertical + moveCamR.normalized * moveHorizontal;

			if (movement.sqrMagnitude > 1)
				movement.Normalize();
			Player.Move(Time.deltaTime * movement, ActionCallbackMove);
			return ;
		}

		private void Jump()
		{
			Player.Jump(ActionCallbackJump);
			return ;
		}

		private void Turn()
		{
			float mouseX = Input.GetAxis("Mouse X") * _mouseRX * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * _mouseRY * Time.deltaTime;

			Camera.Turn(mouseX, mouseY, ActionCallbackTurn);
			return ;
		}
	}
}