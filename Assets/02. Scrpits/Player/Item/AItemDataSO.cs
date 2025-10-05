using UnityEngine;

namespace Player.Item
{
	[CreateAssetMenu(fileName = "New Item Data", menuName = "SO/Data - Item")]
	public abstract class AItemDataSO : ScriptableObject
	{
		public string itemName;
		public string tooltip;
		public Sprite itemSprite;
		public GameObject itemPrefab;

		public abstract AItem CreateItem();
	}
}
