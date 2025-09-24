using Config;
using System;
using UnityEngine;

namespace Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("Player Object")]
		[SerializeField]
		private PlayerModel _playerModel;
		[SerializeField]
		private PlayerCamera _playerCamera;

		[Header("Game Charactor Prefabs")]
		[SerializeField]
		private GameObject[] _charactorPrefabs;

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
		public event Action<Index, bool> ActionCallbackTrySkill;

		private void Awake()
		{
			SetCharactor(0);
			return ;
		}

		void Update()
		{
			Move(Time.deltaTime);
			UseSkill();
			if (ConfigUserInput.Instance.GetKeyDown("keyJump"))
				Jump();
			if (isFixedCursor)
				Turn(Time.deltaTime);
			return ;
		}

		public void SetCharactor(int index)
		{
			if (index >= _charactorPrefabs.Length)
				throw (new ArgumentOutOfRangeException());
			if (_playerModel != null)
				Destroy(_playerModel.gameObject);
			GameObject obj = Instantiate(_charactorPrefabs[index], transform);

			_playerModel = obj.GetComponent<PlayerModel>();
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

		private void Move(float timeSecond)
		{
			float moveHorizontal = ConfigUserInput.Instance.GetAxis("Horizontal", timeSecond);
			float moveVertical = ConfigUserInput.Instance.GetAxis("Vertical", timeSecond);
			Vector3 moveCamR = Vector3.Scale(Camera.transform.right, new Vector3(1, 0, 1));
			Vector3 moveCamF = Vector3.Scale(Camera.transform.forward, new Vector3(1, 0, 1));
			Vector3 movement = moveCamF.normalized * moveVertical + moveCamR.normalized * moveHorizontal;

			if (movement.sqrMagnitude > 1)
				movement.Normalize();
			Player.Move(transform, Time.deltaTime * movement, ConfigUserInput.Instance.GetKey("keySprint"), ActionCallbackMove);
			return ;
		}

		private void UseSkill()
		{
			short index = -1;
			bool trySkill;

			if (ConfigUserInput.Instance.GetKeyDown("keySkill1"))
				index = 0;
			if (ConfigUserInput.Instance.GetKeyDown("keySkill1"))
				index = 1;
			if (ConfigUserInput.Instance.GetKeyDown("keySkill1"))
				index = 2;
			if (ConfigUserInput.Instance.GetKeyDown("keySkill1"))
				index = 3;
			trySkill = Player.UseSkill(index);
			if (index != -1)
				ActionCallbackTrySkill?.Invoke(index, trySkill);
			return ;
		}

		private void Jump()
		{
			Player.Jump(ActionCallbackJump);
			return ;
		}

		private void Turn(float timeSecond)
		{
			float mouseX = Input.GetAxis("Mouse X") * _mouseRX * timeSecond;
			float mouseY = Input.GetAxis("Mouse Y") * _mouseRY * timeSecond;

			Player.Rotate(transform, mouseX);
			Player.SetAngle(Camera.Turn(transform, mouseY));
			ActionCallbackTurn?.Invoke();
			return ;
		}
	}
}