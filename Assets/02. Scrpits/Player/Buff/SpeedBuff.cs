using Player;
using Player.Buff;
using Player.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : ABuffPlayer
{
    private float buffAmount = 1.0f;
    private float baseMoveSpeed = 0f;
    private float baseSprintSpeed = 0f;

    private float buffMoveSpeedAmount = 0f;
    private float buffSprintSpeedAmount = 0f;
    private bool isBuff = true;

    public SpeedBuff(PlayerModel model, float remainSecond, string desc,BuffType buffType, float buffAmount, bool isBuff)
        : base(model, remainSecond, desc)
    {
        this.Type = buffType;
        this.buffAmount = buffAmount;
        this.isBuff = isBuff;
    }

    public override void OnEnable()
    {

        target.Stat.AddCalculateStat(SetBuff);
    }

    public override void OnDisable()
    {
        target.Stat.RemoveCalculateStat(SetBuff);
    }

    public void SetBuff(ref SPlayerStat buffStat)
    {
        buffStat.speedMove = buffAmount;
        buffStat.speedSprint = buffAmount;
    }
}
