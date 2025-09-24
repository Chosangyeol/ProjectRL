namespace Player.Item
{
	public abstract class AItem
	{
		protected ItemDataSO itemData;
		protected string desc;

		public AItem(ItemDataSO itemData)
		{
			return ;
		}

		public abstract void OnAddInventory();

		public abstract bool OnUpdateInventory();

		public abstract void OnRemoveInventory();

		public string GetDesc()
		{
			return (desc);
		}
	}
}
