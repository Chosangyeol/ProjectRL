using UnityEngine;

namespace Player.Item
{
	public class DropItemModel : MonoBehaviour
	{
		private ItemDataSO _itemDataSO;

		public void Init(ItemDataSO item)
		{
			if (item == null)
			{
				Destroy(gameObject);
				return ;
			}
			_itemDataSO = item;
			Instantiate(item.itemPrefab, transform);
			return ;
		}


	}
}
