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

    Condition hp => uiCondition.hp;
    Condition mp => uiCondition.mp;
    Condition exp => uiCondition.exp;

    public int level = 1;

    public event Action onTakeDamage;
    public event Action onLevelUP;

    private float hpTimer;
    private float mpTimer;
    private float expTimer;


    private void Update()
    {
        hpTimer += Time.deltaTime;
        
        if(hpTimer >= 1f)
        {
            hp.Subtract(1);
            hpTimer = 0f;
        }

        mpTimer += Time.deltaTime;

        if(mpTimer >= 0.5f)
        {
            mp.Subtract(1);
            mpTimer = 0f;
        }

        expTimer += Time.deltaTime;

        if(expTimer >= 1f)
        {
            exp.Add(5);
            expTimer = 0f;
        }


        if(hp.curValue <= 0)
        {
            Die();
        }

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
        Debug.Log($"레벨 업! 현재 레벨: {level}");

        exp.Set(0);
        onLevelUP?.Invoke();
    }

    public void Die()
    {
        Debug.Log("플레이어 사망!");
    }
}
