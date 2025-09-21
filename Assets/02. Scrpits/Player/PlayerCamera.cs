using Config;
using System;
using UnityEngine;

namespace Player
{
	public class PlayerCamera : MonoBehaviour
	{
		[SerializeField]
		private Camera _playerCamera;
		[SerializeField]
		private Transform _cameraTarget;

		void Update()
		{
			SetCamPos();
			return ;
		}

		public void Turn(float x, float y, Action callback = null)
		{
			if (ConfigUserInput.Instance.isAxisYFlipped)
				transform.Rotate(Vector3.right, y, Space.Self);
			else
				transform.Rotate(Vector3.left, y, Space.Self);
			transform.Rotate(Vector3.up, x, Space.World);
			callback?.Invoke();
			return;
		}

		private void SetCamPos()
		{
			RaycastHit hit;
			float distance = Vector3.Distance(transform.position, _cameraTarget.position);

			if (Physics.Raycast(transform.position, _cameraTarget.position - transform.position, out hit, distance))
			{
				_playerCamera.transform.position = hit.point; // 벽에 부딪힌 지점
			}
			else
			{
				_playerCamera.transform.position = _cameraTarget.position; // 정상 거리 유지
			}
			return ;
		}
	}
}