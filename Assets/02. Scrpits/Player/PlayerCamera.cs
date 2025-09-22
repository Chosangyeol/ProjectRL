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

		private float nowX = 0f;
		private float minX = -80f;
		private float maxX = 70f;

		void Update()
		{
			SetCamPos();
			return ;
		}

		public Vector3 Turn(Transform parent, float y)
		{
			Vector3 rot = parent.rotation.eulerAngles;

			if (ConfigUserInput.Instance.isAxisYFlipped)
				y = - y;
			nowX -= y;
			nowX = Mathf.Clamp(nowX, minX, maxX);
			transform.rotation = Quaternion.Euler(nowX, rot.y, 0f);
			return (transform.rotation.eulerAngles);
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