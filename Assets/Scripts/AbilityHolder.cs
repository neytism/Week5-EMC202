using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

//
//  Copyright © 2022 Kyo Matias & Nate Florendo. All rights reserved.
//  

public class AbilityHolder : MonoBehaviour
{
    //abilities object
    [SerializeField] private AbilityManager _ability1;
    [SerializeField] private AbilityManager _ability2;
    [SerializeField] private AbilityManager _ability3;
    private AbilityManager _ability;
    
    //ability cooldown system
    private float _cooldownTime;
    private float _duration;
    private float _activeTime;
    [SerializeField] private Image _CDBar;
    private float _cooldown;
    private bool _isCoolDown;
    
    AbilityState state = AbilityState.ready;

    public KeyCode key;
    
    public bool IsCoolDown
    {
        get => _isCoolDown;
        set => _isCoolDown = value;
    }
    
    public float ActiveTime => _activeTime;

    private void Awake()
    {
        _ability = SelectAbilityType(SaveManager.Instance.GetSelectedCharacter);
        _duration = _ability.ActiveTime;
        _cooldown = _ability.Cooldown;
        _CDBar.fillAmount = 1;
    }

    // for updating cooldowns
    void Update()
    {
        switch (state)
        {
           case AbilityState.ready:
               if (Input.GetKeyDown(key))
               {
                   _ability.Activate(gameObject);
                   state = AbilityState.active;
                   _activeTime = _ability.ActiveTime;
               }
               break;
           case AbilityState.active:
               if (_activeTime > 0)
               {
                   _activeTime -= Time.deltaTime;
                   _CDBar.fillAmount = _activeTime;

               }
               else
               {
                   state = AbilityState.cooldown;
                   _cooldownTime = _ability.Cooldown;
               }
               break;
           case AbilityState.cooldown:
               if (_cooldownTime > 0)
               {
                   _cooldownTime -= Time.deltaTime;
                   _CDBar.fillAmount = -(_cooldownTime - _ability.Cooldown) /  _ability.Cooldown;
               }
               else
               {
                   state = AbilityState.ready;
                   Debug.Log("Ability Ready");
               }
               break; 
        }
    }

    //for swift dash
    public void BeInvincible()
    {
        StartCoroutine(InvincibilityTime());
        
    }
    
    IEnumerator InvincibilityTime()
    {
        gameObject.GetComponent<Player>().IsInvincible = true;
        Debug.Log("isinvincible");
        yield return new WaitForSeconds(gameObject.GetComponent<AbilityHolder>()._duration + 1);
        gameObject.GetComponent<Player>().IsInvincible  = false;
        Debug.Log("isNotinvincible");
    }
    
    //for laser

    public void TurnOnLaser()
    {
        StartCoroutine(LaserTime());
    }
    IEnumerator LaserTime()
    {
        
        gameObject.GetComponent<Player>().Beam.SetActive(true);
        Debug.Log("Laser on");
        yield return new WaitForSeconds(gameObject.GetComponent<AbilityHolder>()._duration);
        gameObject.GetComponent<Player>().Beam.SetActive(false);
        Debug.Log("Laser off");
    }
    
    //for EMP
    
    public void Explode()
    {
        StartCoroutine(ExplosionTime());
    }
    
    IEnumerator ExplosionTime()
    {
        gameObject.GetComponent<Player>().AOE.SetActive(true);
        Debug.Log("Explode Start");
        yield return new WaitForSeconds(gameObject.GetComponent<AbilityHolder>()._duration);
        gameObject.GetComponent<Player>().AOE.SetActive(false);
        Debug.Log("Explode End");
    }

    //Returns selected ability from player manager
    private AbilityManager SelectAbilityType(int index)
    {
        
        switch (index)
        {
            case 0:
                return _ability1;
            case 1:
                return _ability2;
            case 2:
                return _ability3;
        }

        return _ability1;
    }

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
}
