public interface IDamagable : IHittable
{
    void TakeDamage(int damageAmount, float knockbackThrust);
}
