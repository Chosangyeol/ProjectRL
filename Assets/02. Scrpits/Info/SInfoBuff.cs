using UnityEngine;

// TODO!
namespace Info
{
	public struct SInfoBuff : IInfo<GameObject, GameObject>
	{
		public GameObject Source { get; private set; }
		public GameObject Target { get; private set; }
		public BuffType type;
		public float timeSecond;
	}
}