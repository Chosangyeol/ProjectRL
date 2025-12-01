using UnityEngine;

namespace Player.Item
{
	public class DropItemModel : PoolableMono, IInteractable
	{
		[SerializeField]
		private AItemDataSO _itemDataSO;
        public AItem _item { get; private set; }
        public string interactName { get; }

        public bool isInRange = false;
        public Transform player;

        private void OnEnable()
        {
            _item = _itemDataSO.CreateItem();
            Debug.Log(_item.itemData.itemName);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = true;
                player = other.transform;
                OnFocus();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = false;
                player = null;
                OnUnFocus();
            }
        }

        #region Interact
        public void OnFocus()
        {
            Debug.Log("아이템 줍기 가능");
        }

        public void OnUnFocus()
        {
            Debug.Log("아이템 줍기 불가능");
        }

        public void OnInteract()
        {
            PlayerModel model = player.GetComponentInChildren<PlayerModel>();
            TryPickUp(model);
        }

        private void TryPickUp(PlayerModel model)
        {
            if (_itemDataSO == null)
                return;

            model.AddItem(_item);
            PoolManager.Instance.Push(this);
            // 오브젝트풀로 돌리기
        }
        #endregion
    }
}
