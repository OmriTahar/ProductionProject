using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    [Header("Unit Settings")]
    [SerializeField][ReadOnlyInspector] internal float _unitHP = 0;
    [SerializeField] protected float _unitMaxHP, _unitAttackRange;
    [SerializeField] Image _healthBarBG, _healthBar;

    [Header("Stun Settings")]
    public bool IsStunned;
    public ParticleSystem _stunEffect;

    public static event Action OnBunnyKilled;


    protected virtual void Start()
    {
        _unitHP = _unitMaxHP;

        if (_healthBar)
            _healthBar.fillAmount = _unitHP / _unitMaxHP;
    }

    public void RecieveDamage(IAttackable<Unit> enemy)
    {
        enemy.Attack(this);

        if (_healthBar)
            _healthBar.fillAmount = _unitHP / _unitMaxHP;

        if (gameObject.CompareTag("Player"))
            PlayerData.Instance.AnimatorGetter.SetTrigger("GotHit");

        CheckDeath();
    }

    protected virtual void OnDeath()
    {
        PlayerData.Instance.AddScore();
        OnBunnyKilled?.Invoke();      
        Destroy(gameObject);
    }

    private void CheckDeath()
    {
        if (_unitHP <= 0)
        {
            OnDeath();
        }
    }
}