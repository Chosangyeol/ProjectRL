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

		public abstract void OnAddInventory(PlayerModel model);

		public abstract bool OnUpdateInventory(PlayerModel model, float delta);

		public abstract void OnRemoveInventory(PlayerModel model);
	}
}
