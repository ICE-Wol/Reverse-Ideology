namespace _Scripts {
    public abstract class BulletDestroyBehaviour {
        public DestroyType type;
        public Bullet bullet;
        public int timer;

        public abstract void DestroyBehaviour();
    }
}