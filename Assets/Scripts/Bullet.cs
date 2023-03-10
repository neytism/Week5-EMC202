using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//
//  Copyright © 2022 Kyo Matias & Nate Florendo. All rights reserved.
//  

public class Bullet : MonoBehaviour
{
   private void OnEnable()
   {
      StartCoroutine(BulletLife(gameObject));
   }

   private void OnCollisionEnter2D(Collision2D col) 
   {
      gameObject.SetActive(false);
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      gameObject.SetActive(false);
   }
   
   IEnumerator BulletLife(GameObject bullet)
   {
      yield return new WaitForSeconds(3);
      bullet.SetActive(false);
   }
}
