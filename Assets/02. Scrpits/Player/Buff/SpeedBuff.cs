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
    private float baseSprintSpeedAmount = 0f;
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
        baseMoveSpeed = target.Stat.GetSpeed(false);
        baseSprintSpeed = target.Stat.GetSpeed(true);

        buffMoveSpeedAmount = baseMoveSpeed * buffAmount;
        baseSprintSpeedAmount = baseSprintSpeed* baseSprintSpeed;        

        if (isBuff)
        {
            SPlayerStat buffStat = new SPlayerStat
            {
                speedMove = buffMoveSpeedAmount,
                speedSprint = baseSprintSpeedAmount
            };

            target.Stat.AddStat(buffStat);

        }
        else
        {
            SPlayerStat buffStat = new SPlayerStat
            {
                speedMove = -buffMoveSpeedAmount,
                speedSprint = -baseSprintSpeedAmount
            };

            target.Stat.AddStat(buffStat);
        }
    }

    public override void OnDisable()
    {
        if (isBuff)
        {
            SPlayerStat buffStat = new SPlayerStat
            {
                speedMove = -buffMoveSpeedAmount,
                speedSprint = -baseSprintSpeedAmount
            };

            target.Stat.AddStat(buffStat);

        }
        else
        {
            SPlayerStat buffStat = new SPlayerStat
            {
                speedMove = buffMoveSpeedAmount,
                speedSprint = baseSprintSpeedAmount
            };

            target.Stat.AddStat(buffStat);
        }
    }
}
