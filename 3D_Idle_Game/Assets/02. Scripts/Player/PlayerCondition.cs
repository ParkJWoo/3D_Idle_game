using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    public Condition hp => uiCondition.hp;
    public Condition mp => uiCondition.mp;
    public Condition exp => uiCondition.exp;

    public int level = 1;

    public event Action onTakeDamage;
    public event Action onLevelUP;

    private float hpTimer;
    private float mpTimer;
    private float expTimer;


    private void Update()
    {
        if(exp.curValue >= exp.maxValue)
        {
            LevelUP();
        }
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
    }

    public void TakePhysicalDamage(int damage)
    {
        hp.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public bool UseMP(int amount)
    {
        if(mp.curValue < amount)
        {
            return false;
        }

        mp.Subtract(amount);

        return true;
    }

    public void RecoverMP(int amount)
    {
        mp.Add(amount);
    }

    public void GainExp(int amount)
    {
        exp.Add(amount);
    }

    private void LevelUP()
    {
        level++;

        exp.Set(0);
        onLevelUP?.Invoke();
    }

    public void Die()
    {
        Debug.Log("플레이어 사망!");
    }
}
