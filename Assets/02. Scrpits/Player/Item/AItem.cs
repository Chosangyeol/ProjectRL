namespace Player.Item
{
	public abstract class AItem
	{
		public AItemDataSO itemData;

		public AItem(AItemDataSO itemData)
		{
			this.itemData = itemData;
			return ;
		}

		public abstract void OnAddInventory(PlayerModel model);

		public abstract bool OnUpdateInventory(PlayerModel model, float delta);

		public abstract void OnRemoveInventory(PlayerModel model);
	}
}
