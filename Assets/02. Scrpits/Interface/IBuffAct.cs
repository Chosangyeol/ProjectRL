using UnityEngine;

public interface IBuffAct
{
	public BuffType Type { get; }

	public string Desc { get; }

	public void OnEnable();
	public bool Update(float delta);
	public void OnDisable();
	public void SetUnactive();
}
