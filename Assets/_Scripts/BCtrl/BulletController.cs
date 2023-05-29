using System;
using _Scripts;
using _Scripts.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.BCtrl {
    [Serializable]
    public abstract class BulletController : MonoBehaviour {
        public bool isActivated;
        
        public int ways;
        public float initDeg;
        public float intervalDeg;
        public float genRadius;

        public int startTime;
        public int intervalTime;
        public int endTime;

        public int waveCount;
        public int maxWave;
        
        public BulletData mainBulletData;
        public StepType mainStepType;
        [SerializeReference] public BulletStepBehaviour mainStepBehaviour;

        [HideInInspector] public int timer;

        public void Activate() {
            isActivated = true;
        }

        public Vector3 Snip() =>
            (GameManager.Player.transform.position - transform.position).normalized;

        public abstract void GenerateBullets();

        public void FixedUpdate() {
            if (isActivated) {
                if (timer >= startTime && timer <= endTime && waveCount <= maxWave) {
                    if ((timer - startTime) % intervalTime == 0) {
                        GenerateBullets();
                    }
                }
                timer++;

                if (timer > endTime && waveCount > maxWave) {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}

