using System;
using System.Collections.Generic;
using _Scripts.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts {
    public class BulletController : MonoBehaviour {
        protected Bullet[] bullets;
        protected bool isActivated;
        protected int ways;
        protected int timer;
        protected float initDeg;
        protected float intervalDeg;
        protected float genRadius;
        protected BulletData bulletData;

        public void Activate(int ways, float initDeg, float intervalDeg,float genRadius, BulletData data) {
            this.genRadius = genRadius;
            this.ways = ways;
            this.initDeg = initDeg;
            this.intervalDeg = intervalDeg;
            bulletData = data;
            isActivated = true;
            GenerateBullets();
        }
        public void Activate(int ways, float initDeg, float intervalDeg, BulletData data) {
            this.genRadius = 0;
            this.ways = ways;
            this.initDeg = initDeg;
            this.intervalDeg = intervalDeg;
            bulletData = data;
            isActivated = true;
            GenerateBullets();
        }

        public void Activate(int ways, float initDeg, BulletData data) {
            this.genRadius = 0;
            this.ways = ways;
            this.initDeg = initDeg;
            this.intervalDeg = 360f / ways;
            bulletData = data;
            isActivated = true;
            GenerateBullets();
        }
        
        public void UpdateBullets() {
            int activeCount = 0;
            for(int i = 0;i < ways;i++) {
                if (bullets[i]) {
                    activeCount++;
                }
            }

            if (activeCount == 0) {
                //Debug.Log("Dead");
                Destroy(this.gameObject);
            }
        }

        public void GenerateBullets() {
            bullets = new Bullet[ways];
            for (int i = 0; i < ways; i++) {
                var p = BulletManager.Manager.GetBullet();
                bulletData.isAuto = true;
                bulletData.rotation = initDeg + intervalDeg * i;
                bulletData.direction = Calc.Deg2Dir(bulletData.rotation);
                p.SetData(bulletData);
                p.SetParent(this, i);
                p.transform.position = transform.position + genRadius * bulletData.direction;
                bullets[i] = p;
            }
        }

        public void CheckPlayerCollision(PlayerController player, float r) {
            for(int i = 0;i < ways;i++) {
                if (bullets[i] && bullets[i].CheckCollision(player.gameObject, r)) {
                    bullets[i].Release();
                    player.GetHit();
                    break;
                }
            }
        }

        public void ReleaseBulletInArray(int ord) {
            bullets[ord] = null;
        }

        private void FixedUpdate() {
            if(isActivated)
                UpdateBullets();
        }
    }
}
