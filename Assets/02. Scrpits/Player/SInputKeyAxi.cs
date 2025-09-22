using JetBrains.Annotations;
using System;
using UnityEngine;

namespace Player
{
	[Serializable]
	public struct SInputKeyAxe
	{
		public float axis;
		public float deadZone;
		public float sensitivity;
		public float gravity;

		public void Init(float deadZone, float sensitivity, float gravity)
		{
			this.deadZone = deadZone;
			this.sensitivity = sensitivity;
			this.gravity = gravity;
			return ;
		}

		public float GetAxis(KeyCode positive, KeyCode negative, float deltaTime)
		{
			float value = 0f;

			if (Input.GetKey(positive)) value += 1f;
			if (Input.GetKey(negative)) value -= 1f;

			if (Mathf.Abs(value) < deadZone)
				value = 0f;

			// 보정 처리
			if (value != 0)
				axis = Mathf.MoveTowards(axis, value, sensitivity * deltaTime);
			else
				axis = Mathf.MoveTowards(axis, 0, gravity * deltaTime);

			return (axis);
		}
	}
}
