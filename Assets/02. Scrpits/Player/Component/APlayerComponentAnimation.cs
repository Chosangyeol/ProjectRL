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

		public void SetFloat(string key, float value)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetFloat(hashedParameters[key], value);
			}
			return ;
		}

		public void SetBool(string key, bool value)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetBool(hashedParameters[key], value);
			}
			return ;
		}

		public void SetInteger(string key, int value)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetInteger(hashedParameters[key], value);
			}
			return ;
		}

		public void SetTrigger(string key)
		{
			if (hashedParameters.ContainsKey(key))
			{
				animator.SetTrigger(hashedParameters[key]);
			}
			return ;
		}

		public abstract void Update(float delta);
	}
}
