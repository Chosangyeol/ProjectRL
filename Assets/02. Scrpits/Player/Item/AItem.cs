namespace Player.Item
{
	public abstract class AItem
	{
		public ItemDataSO itemData;

		public AItem(ItemDataSO itemData)
		{
			this.itemData = itemData;
			return ;
		}

		public abstract void OnAddInventory();

		public abstract bool OnUpdateInventory();

		public abstract void OnRemoveInventory();
	}
}
