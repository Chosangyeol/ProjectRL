using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Component
{
	public abstract class APlayerComponentAnimation
	{
		public PlayerModel playerModel;
		public Animator animator;

		protected Dictionary<string, int> hashedParameters = new Dictionary<string, int>();

		public Vector3		deltaPosition { get => animator.deltaPosition; }
		public Quaternion	deltaRotation { get => animator.deltaRotation; }
		public Vector3		velocity { get => animator.velocity; }
		public Vector3		angularVelocity { get => animator.angularVelocity; }
		public Vector3		rootPosition { get => animator.rootPosition; }
		public Quaternion	rootRotation { get => animator.rootRotation; }
		public float		gravityWeight { get => animator.gravityWeight; }

		public APlayerComponentAnimation(PlayerModel model)
		{
			playerModel = model;
			animator = playerModel.GetComponent<Animator>();
			if (animator == null)
				throw (new Exception("Cannot Find Player Animator"));
			for (int i = 0; i < animator.parameterCount; i++)
			{
				string name = animator.parameters[i].name;
				
				if (!hashedParameters.ContainsKey(name))
					hashedParameters.Add(name, Animator.StringToHash(name));
			}
			return ;
		}
		
		public float GetFloat(string key)
		{
			if (hashedParameters.ContainsKey(key))
			{
				return (animator.GetFloat(hashedParameters[key]) );
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public void SetFloat(string key, float value)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetFloat(hashedParameters[key], value);
				return ;
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public bool GetBool(string key)
		{
			if (hashedParameters.ContainsKey(key))
			{
				return (animator.GetBool(hashedParameters[key]) );
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public void SetBool(string key, bool value)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetBool(hashedParameters[key], value);
				return ;
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public int GetInteger(string key)
		{
			if (hashedParameters.ContainsKey(key))
			{
				return (animator.GetInteger(hashedParameters[key]));
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public void SetInteger(string key, int value)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetInteger(hashedParameters[key], value);
				return ;
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public void SetTrigger(string key)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetTrigger(hashedParameters[key]);
				return ;
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public void ResetTrigger(string key)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.ResetTrigger(hashedParameters[key]);
				return ;
			}
			throw (new ArgumentException($"key [{key}] is not found"));
		}

		public float GetIKPositionWeight(AvatarIKGoal goal)
		{
			return (animator.GetIKPositionWeight(goal) );
		}

		public float GetIKRotationWeight(AvatarIKGoal goal)
		{
			return (animator.GetIKRotationWeight(goal));
		}

		public void SetIKPositionWeight(AvatarIKGoal goal, float value)
		{
			animator.SetIKPositionWeight(goal, value);
			return ;
		}

		public void SetIKRotationWeight(AvatarIKGoal goal, float value)
		{
			animator.SetIKRotationWeight(goal, value);
			return ;
		}

		public Vector3 GetIKPosition(AvatarIKGoal goal)
		{
			return (animator.GetIKPosition(goal));
		}

		public Quaternion GetIKRotation(AvatarIKGoal goal)
		{
			return (animator.GetIKRotation(goal));
		}

		public void SetIKPosition(AvatarIKGoal goal, Vector3 goalPosition)
		{
			animator.SetIKPosition(goal, goalPosition);
			return ;
		}

		public void SetIKRotation(AvatarIKGoal goal, Quaternion goalRotation)
		{
			animator.SetIKRotation(goal, goalRotation);
			return ;
		}

		public Vector3 GetIKHintPosition(AvatarIKHint hint)
		{
			return (animator.GetIKHintPosition(hint) );
		}

		public float GetIKHintPositionWeight(AvatarIKHint hint)
		{
			return (animator.GetIKHintPositionWeight(hint));
		}

		public void SetIKHintPosition(AvatarIKHint hint, Vector3 hintPosition)
		{
			animator.SetIKHintPosition(hint, hintPosition);
			return ;
		}

		public void SetIKHintPositionWeight(AvatarIKHint hint, float value)
		{
			animator.SetIKHintPositionWeight(hint, value);
			return ;
		}

		public void SetLookAtPosition(Vector3 lookAtPosition)
		{
			animator.SetLookAtPosition(lookAtPosition);
			return ;
		}

		public void SetLookAtWeight(float weight)
		{
			animator.SetLookAtWeight(weight);
			return ;
		}

		public void SetLookAtWeight(float weight, float bodyWeight)
		{
			animator.SetLookAtWeight(weight, bodyWeight);
			return ;
		}

		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight)
		{
			animator.SetLookAtWeight(weight, bodyWeight, headWeight);
			return ;
		}

		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight);
			return ;
		}

		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight, float eyesWeight, float clampWeight)
		{
			animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
			return ;
		}

		public int GetLayerIndex(string layerName)
		{
			return (animator.GetLayerIndex(layerName));
		}

		public string GetLayerName(int layerIndex)
		{
			return (animator.GetLayerName(layerIndex));
		}

		public float GetLayerWeight(int layerIndex)
		{
			return (animator.GetLayerWeight(layerIndex));
		}

		public void SetLayerWeight(int layerIndex, float weight)
		{
			animator.SetLayerWeight(layerIndex, weight);
			return ;
		}

		public abstract void Update(float delta);
	}
}
