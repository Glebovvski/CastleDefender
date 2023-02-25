using System;

public interface IDamageReceiver
{
    public event Action OnTakeDamage;
    public event Action OnDeath;
    public void TakeDamage(float value);
}
