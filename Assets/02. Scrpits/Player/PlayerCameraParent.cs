using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerCameraParent : MonoBehaviour
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

		public void SetCamPos()
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