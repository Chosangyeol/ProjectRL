using Info;
using System;
using System.Collections.Generic;

namespace Player.Item
{
	[Serializable]
	public class Inventory
	{
		private readonly PlayerModel playerModel;

		private List<AItem> items;

		public List<AItem> Items { get => items; }

		public event Action<AItem> ActionBeforeAddItem;
		public event Action<AItem> ActionAfterAddItem;
		public event Action<AItem> ActionBeforeRemoveItem;
		public event Action<AItem> ActionAfterRemoveItem;

		public Inventory(PlayerModel playerModel)
		{
			this.playerModel = playerModel;
			items = new List<AItem>();
			return ;
		}

		public void AddItem(AItem item)
		{
			ActionBeforeAddItem?.Invoke(item);
			item.OnAddInventory(playerModel);
			items.Add(item);
			ActionAfterAddItem?.Invoke(item);
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
				ActionBeforeRemoveItem?.Invoke(item);
				item.OnRemoveInventory(playerModel);
                items.Remove(item);
				ActionAfterRemoveItem?.Invoke(item);
				return (true);
			}
			return (false);
		}
	}
}
