using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat Instance { get; private set; }

    [Header("Base Stats")]
    public int baseHP = 100;
    public int baseMP = 100;
    public int baseAttackPower = 10;

    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    public int MaxMP { get; private set; }
    public int CurrentMP { get; private set; }

    public int CurrentAttackPower { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MaxHP = baseHP;
        MaxMP = baseMP;
        CurrentHP = MaxHP;
        CurrentMP = MaxMP;
        CurrentAttackPower = baseAttackPower;

        var player = CharacterManager.Instance?.Player;

        if(player?.condition != null)
        {
            player.condition.hp.maxValue = MaxHP;
            player.condition.hp.Set(CurrentHP);

            player.condition.mp.maxValue = MaxMP;
            player.condition.mp.Set(CurrentMP);
        }
    }

    //  ��� �������� ���� �� HP ȸ�� �޼���
    public void Heal(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);
        CharacterManager.Instance?.Player?.condition.hp.Set(CurrentHP);
    }

    //  �ٳ��� �������� ���� �� MP ȸ�� �޼���
    public void ManaHeal(int amount)
    {
        CurrentMP = Mathf.Clamp(CurrentMP + amount, 0, MaxMP);
        CharacterManager.Instance?.Player?.condition.mp.Set(CurrentMP);
    }

    //  �ܹ��� �������� ���� �� ���� �ð����� ���ݷ� ��� ������ �����ϴ� �޼���
    public void TemporaryAttackPowerBuff(int amount, float duration)
    {
        StartCoroutine(ApplyAttackPowerBuffCoroutine(amount, duration));
    }

    //  ���� ���� �޼��� �� ���� �ð����ȿ��� �۵��ϵ��� �����߽��ϴ�.
    private IEnumerator ApplyAttackPowerBuffCoroutine(int amount, float duration)
    {
        CurrentAttackPower += amount;
        yield return new WaitForSeconds(duration);
        CurrentAttackPower = Mathf.Max(0, CurrentAttackPower - amount);
    }
}
