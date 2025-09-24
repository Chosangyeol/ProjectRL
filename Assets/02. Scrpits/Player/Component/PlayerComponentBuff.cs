using Info;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Component
{
	// TODO!
	[Serializable]
	public class PlayerComponentBuff
	{
		private readonly PlayerModel	playerModel;
		private List<SInfoBuff>			ListBuff;

		public delegate void BuffHandler(ref SInfoBuff buff);

		public event BuffHandler		ActionBeforeAddBuff;
		public event Action<SInfoBuff>	ActionAfterAddBuff;
		public event BuffHandler		ActionBeforeRemoveBuff;
		public event Action<SInfoBuff>	ActionAfterRemoveBuff;

		public PlayerComponentBuff(PlayerModel model)
		{
			playerModel = model;
			return ;
		}

		public void AddBuff(SInfoBuff info)
		{
			ActionBeforeAddBuff?.Invoke(ref info);
			info.act.OnEnable();
			ListBuff.Add(info);
			ActionAfterAddBuff?.Invoke(info);
		}

		public bool UpdateBuff(float delta)
		{
			bool result = false;

			for (int i = ListBuff.Count - 1; i >= 0; i--)
			{
				SInfoBuff buff = ListBuff[i];
				bool isEnd = buff.act.Update(delta);

				if (isEnd)
				{
					ActionBeforeRemoveBuff?.Invoke(ref buff);
					buff.act.OnDisable();
					ActionAfterRemoveBuff?.Invoke(buff);
					ListBuff.RemoveAt(i);
				}
			}
			return (result);
		}

		public void RemoveBuff(ref SInfoBuff buff)
		{
			buff.act.SetUnactive();
			return ;
		}
	}
}
