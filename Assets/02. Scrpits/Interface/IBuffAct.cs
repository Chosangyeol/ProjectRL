using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public interface IBuffAct
{
	public GameObject Target { get; }

	public string Desc { get; }

	public void OnEnable();
	public bool Update(float delta);
	public void OnDisable();
	public void SetDisable();
}
