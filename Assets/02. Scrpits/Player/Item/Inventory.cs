using Info;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Player.Item
{
	[Serializable]
	public class Inventory
	{
		private readonly PlayerModel playerModel;

		private List<AItem> items;

		public event Action ActionBeforeAddItem;
		public event Action ActionAfterAddItem;
		public event Action ActionBeforeRemoveItem;
		public event Action ActionAfterRemoveItem;

		public Inventory(PlayerModel playerModel)
		{
			this.playerModel = playerModel;
			items = new List<AItem>();
			return ;
		}

		public void AddItem(AItem item)
		{
			ActionBeforeAddItem?.Invoke();
			item.OnAddInventory(playerModel);
			items.Add(item);
			ActionAfterAddItem?.Invoke();
			return ;
		}

		public void UpdateItem(float delta)
		{
			for (int i = 0; i < items.Count; i++)
			{
				AItem item = items[i];

				item.OnUpdateInventory(playerModel, delta);
			}
			return ;
		}

		public bool RemoveItem(AItem item)
		{
			if (items.Contains(item))
			{
				ActionBeforeRemoveItem?.Invoke();
				items.Remove(item);
				ActionAfterRemoveItem?.Invoke();
				return (true);
			}
			return (false);
		}
	}
}
