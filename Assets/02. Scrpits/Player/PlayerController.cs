using Config.UserInput;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		[Header("Player Object")]
		[SerializeField]
		private PlayerModel _playerModelParent;
		[SerializeField]
		private GameObject _playerCameraParent;

		[Header("Movement")]
		[SerializeField]
		private float _moveSpeed;
		[SerializeField]
		private float _jumpForce;

		[Header("Input Mouse")]
		[SerializeField]
		private float _mouseRX;
		[SerializeField]
		private float _mouseRY;

		private Rigidbody _rigid;

		private bool isGrounded = true;
		private bool isFixedCursor = true;
		private bool isAlive = true;
		private bool isMoveable = true;
		
		void Awake()
		{
			_rigid = GetComponent<Rigidbody>();
			return ;
		}

		void Update()
		{
			if (!isAlive || !isMoveable)
				return ;
			Move();
			if (Input.GetKeyDown(UserInputConfig.Instance.keyJump))
				Jump();
			if (isFixedCursor)
				Turn();
			return ;
		}

		public void SetMoveable(bool active)
		{
			isMoveable = active;
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
			Vector3 moveCamF = Vector3.Scale(_playerCameraParent.transform.forward, new Vector3(1, 0, 1));
			Vector3 moveCamR = Vector3.Scale(_playerCameraParent.transform.right, new Vector3(1, 0, 1));
			Vector3 movement = moveCamF * moveVertical + moveCamR * moveHorizontal;

			if (movement.sqrMagnitude > 1)
				movement.Normalize();
			transform.position += _moveSpeed * Time.deltaTime * movement;
			return ;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("Ground"))
			{
				isGrounded = true;
			}
			return ;
		}

		private void Jump()
		{
			if (isGrounded)
			{
				_rigid.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
				isGrounded = false;
			}
			return ;
		}

		private void Turn()
		{
			float mouseX = Input.GetAxis("Mouse X") * _mouseRX * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * _mouseRY * Time.deltaTime;

			if (UserInputConfig.Instance.isAxisYFlipped)
				_playerCameraParent.transform.Rotate(Vector3.right, mouseY);
			else
				_playerCameraParent.transform.Rotate(Vector3.left, mouseY);
			transform.Rotate(Vector3.up, mouseX);
			return ;
		}
	}
}