using Info;
using UnityEngine;

// TODO!
public struct SInfoInt : IInfo<GameObject, GameObject>
{
	public GameObject Source { get; private set; }
	public GameObject Target { get; private set; }
	public int value;

	public SInfoInt(GameObject source, GameObject target, int value = 0)
	{
		Source = source;
		Target = target;
		this.value = value;
		return ;
	}
}
