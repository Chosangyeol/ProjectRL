using Info;
using Player;
using Player.Buff;
using Player.Component;

public class PlayerBuffTimeToHunt : ABuffPlayer
{
	private ElementType typeNormal = RegistryElement.Get("NormalAttack");
	private int successAttackCount;

	public PlayerBuffTimeToHunt(PlayerModel model, float remainSecond, string desc, BuffType buffType)
		: base(model, remainSecond, desc)
	{
		this.Type = buffType;
		return;
	}

	public override void OnEnable()
	{
		float time = target.GetAttackCooltime();

		time /= 1.5f;
		successAttackCount = 0;
		target.Stat.AddCalculateStat(SetBuff);
		target.SetAttackCooltime(time);
		target.ActionOnBeforeDamage += SetPowerUp;
		target.ActionOnAfterDamage += CountAttack;
		return;
	}

	public override void OnDisable()
	{
		target.Stat.RemoveCalculateStat(SetBuff);
		target.SetAttackCooltime();
		target.ActionOnBeforeDamage -= SetPowerUp;
		target.ActionOnAfterDamage -= CountAttack;
		return;
	}

	public void SetBuff(ref SPlayerStat stat)
	{
		stat.critDamagePercent += 0.5f;
		return;
	}

	public void CountAttack(SInfoAttack info)
	{
		if (info.type == typeNormal)
			successAttackCount++;
		return ;
	}

	public void SetPowerUp(ref SInfoAttack info)
	{
		if (successAttackCount >= 10 && info.type == typeNormal)
		{
			successAttackCount -= 10;
			info.damage *= 12;
		}
		return ;
	}
}
