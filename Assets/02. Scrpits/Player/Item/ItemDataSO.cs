using UnityEngine;

namespace Player.Item
{
	[CreateAssetMenu(fileName = "New Item Data", menuName = "SO/Data - Item")]
	public class ItemDataSO : ScriptableObject
	{
		public string itemName;
		public string tooltip;
		public int itemIndex;
		public Sprite itemSprite;
		public GameObject itemPrefab;
	}
}
