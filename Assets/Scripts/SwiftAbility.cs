using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//
//  Copyright © 2022 Kyo Matias & Nate Florendo. All rights reserved.
//  


[CreateAssetMenu]
public class SwiftAbility : AbilityManager

// Swift ability is the Projectile Class
{
    [SerializeField] private float DashVelocity ;

    public override void Activate(GameObject parent)
    {
        PlayerController movement = parent.GetComponent<PlayerController>();
        AbilityHolder player = parent.GetComponent<AbilityHolder>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        
        AudioManager.Instance.PlayOnce(AudioManager.Sounds.SwiftSound);
        
        player.IsCoolDown = true;
        player.BeInvincible();
        
        //rb.velocity = new Vector2(rb.velocity.x, movement.MoveSpeed * DashVelocity);
        rb.velocity = movement._moveDirection.normalized * DashVelocity;
        
        //rb.velocity = new Vector2(movement.MoveSpeed * DashVelocity, rb.velocity.y);// DO NOT TOUCH, WORKING BEAUTIFULLY
        Debug.Log("Dash ABILITY ");
    }

    
}
