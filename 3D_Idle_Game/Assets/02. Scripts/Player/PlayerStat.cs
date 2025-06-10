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

    //  사과 아이템을 먹을 시 HP 회복 메서드
    public void Heal(int amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, MaxHP);
        CharacterManager.Instance?.Player?.condition.hp.Set(CurrentHP);
    }

    //  바나나 아이템을 먹을 시 MP 회복 메서드
    public void ManaHeal(int amount)
    {
        CurrentMP = Mathf.Clamp(CurrentMP + amount, 0, MaxMP);
        CharacterManager.Instance?.Player?.condition.mp.Set(CurrentMP);
    }

    //  햄버거 아이템을 먹을 시 일정 시간동안 공격력 상승 버프를 적용하는 메서드
    public void TemporaryAttackPowerBuff(int amount, float duration)
    {
        StartCoroutine(ApplyAttackPowerBuffCoroutine(amount, duration));
    }

    //  버프 적용 메서드 → 일정 시간동안에만 작동하도록 구현했습니다.
    private IEnumerator ApplyAttackPowerBuffCoroutine(int amount, float duration)
    {
        CurrentAttackPower += amount;
        yield return new WaitForSeconds(duration);
        CurrentAttackPower = Mathf.Max(0, CurrentAttackPower - amount);
    }
}
