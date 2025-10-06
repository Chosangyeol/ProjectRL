using Config;
using System;
using System.Text;
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

			if (ConfigUserInput.Instance.input.isAxisYFlipped)
				y = - y;
			nowX -= y;
			nowX = Mathf.Clamp(nowX, minX, maxX);
			transform.rotation = Quaternion.Euler(nowX, rot.y, 0f);
			return (transform.rotation.eulerAngles);
		}

		public Vector3 RaycastByAngle(Transform origin, Vector3 direction = default, float distance = 50f)
		{
			return (RaycastByAngle(origin.position, direction, distance));
		}

		public Vector3 RaycastByAngle(Vector3 origin = default, Vector3 direction = default, float distance = 50f)
		{
			Vector3 result;
			RaycastHit hit;
			bool flag;

			if (origin == default)
				origin = transform.position;
			if (direction == default)
				direction = transform.forward;
			flag = Physics.Raycast(origin, direction, out hit, distance);
			if (flag)
			{
				result = hit.point;
			}
			else
			{
				result = origin + direction.normalized * distance;
			}
			return (result);
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