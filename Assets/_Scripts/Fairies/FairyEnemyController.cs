using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using _Scripts.BCtrl;
using Unity.VisualScripting;

namespace _Scripts.Fairies {
    public abstract class FairyEnemyController : MonoBehaviour {
        [SerializeReference] public BulletController mainController;
        [SerializeReference] public BulletController[] controllers;
        public Sprite[] animFairy;
        protected SpriteRenderer spriteRenderer;
        protected int timer;
        protected int frameSpeed;
        protected int idlePointer;
        protected int movePointer;
        protected Vector3 prePosition;
        protected Vector2 direction;
        
        public int health;
        public int maxHealth;
        public float radius;
        public Color color;
        public List<ItemType> itemList;

        public int startFrame;
        public int frameCycle;

        public void TakeDamage(int dam) {
            health -= dam;
            if(health <= 0) Break();
        }

        protected void Break() {
            EnemyManager.Manager.enemyList.Remove(this);
            
            var inst = 
                Instantiate(ParticleManager.Manager.fairyBreakEffect, 
                    transform.position, transform.rotation);
            inst.color = color;
            
            if (itemList.Count == 1) {
                ItemManager.GetItemToPosition(itemList[0], transform.position);    
            }
            else {
                float randR, randD;
                foreach (var type in itemList) {
                    randR = Random.Range(-0.8f, 0.8f);
                    randD = Random.Range(0, 360f);
                    ItemManager.GetItemToPosition(type, transform.position + randR * randD.Deg2Dir3());
                }
            }
            
            Destroy(this.gameObject);
        }
        
        protected void PlayAnim() {
            direction = (transform.position - prePosition).normalized;
            if (timer % frameSpeed == 0) {
                float horVector = direction.x;
                bool hasHorizontalMovement = !horVector.Equal(0f);
                if (hasHorizontalMovement) {
                    movePointer += (int)Mathf.Sign(horVector);
                    bool movePointerReachEdges = (Math.Abs(movePointer) == 8);
                    if (movePointerReachEdges) movePointer -= 4 * Math.Sign(movePointer);
                    spriteRenderer.sprite = animFairy[4 + Math.Abs(movePointer)];
                    spriteRenderer.flipX = (movePointer < 0);
                }
                else {
                    bool remainSideAnimation = (movePointer != 0);
                    if (remainSideAnimation) {
                        movePointer -= Math.Sign(movePointer);
                        spriteRenderer.sprite = animFairy[4 + Math.Abs(movePointer)];
                        spriteRenderer.flipX = (movePointer < 0);
                    }
                    else {
                        idlePointer++;
                        if (idlePointer == 4) idlePointer = 0;
                        spriteRenderer.sprite = animFairy[idlePointer];
                    }
                }
            }
            prePosition = transform.position;
        }

        protected void Initialize() {
            timer = 0;
            frameSpeed = 4;
            movePointer = 0;
            prePosition = transform.position;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected abstract void Movement();
        protected abstract void RegularShoot();
        protected abstract void DeadShoot();
    }
}

#region TestCode

//temp controller = flower bullet set
/*BulletData data = new BulletData();
data.type = BulletType.Point;
data.color = Color.white;

BulletData data2 = new BulletData();
data2.type = BulletType.Rice;
data2.color = Color.blue;
var c = Instantiate(tempController, transform.position, Quaternion.Euler(0, 0, 0));
(c as FlowerBulletSet).Activate(18 * 5,6,2,2f,90f,2f,data,data2);

var d = Instantiate(tempController, transform.position, Quaternion.Euler(0, 0, 0));
(d as FlowerBulletSet).Activate(18 * 5,6,-2,2f,90f,2f,data,data2);*/

//attack here.
//temp controller: BulletController; 
/*if (_timer % 6 == 0) {
    _tempBullet.color = Color.cyan;
    var c = Instantiate(tempController, transform.position,Quaternion.Euler(0f,0f,0f));
    c.Activate(10, 180f * Mathf.Sin(_timer*Mathf.Deg2Rad) + _timer * 4, 7f,0.5f, _tempBullet);
    _tempBullet.color = Color.blue;
    var d = Instantiate(tempController, transform.position,Quaternion.Euler(0f,0f,0f));
    d.Activate(10, 180f * Mathf.Sin(_timer*Mathf.Deg2Rad) + 180f + _timer * 4, 7f,0.5f, _tempBullet);
}

if (_timer % 30 == 0) {
    BulletData data = new BulletData();
    data.type = BulletType.JadeR;
    data.color = Color.green;
    data.speed = 4f;
    var c = Instantiate(tempController, transform.position,Quaternion.Euler(0f,0f,0f));
    c.Activate(48, _timer,data);
}*/

/*if (_timer % 120 == 0) {
    
    ItemManager.Manager.GetItemToPosition(ItemType.Power, transform.position);    
}*/


#endregion