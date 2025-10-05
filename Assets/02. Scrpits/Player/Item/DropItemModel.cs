using UnityEngine;

namespace Player.Item
{
	public class DropItemModel : MonoBehaviour
	{
		private AItemDataSO _itemDataSO;

		public void Init(AItemDataSO item)
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
