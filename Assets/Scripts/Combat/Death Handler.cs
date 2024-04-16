using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    
    
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatterPrefab;
        Health.OnDeath += SpawnDeathVFX;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFX;
    }

    void SpawnDeathSplatterPrefab(Health sender)
    {
        GameObject _newSplatterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, sender.transform.rotation);
        SpriteRenderer _deathSplatterRenderer = _newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger _colorChanger = sender.GetComponent<ColorChanger>();
        Color _currentColor = _colorChanger.DefaultColor;
        _deathSplatterRenderer.color = _currentColor;
        _newSplatterPrefab.transform.SetParent(this.transform);
    }

    void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFX, sender.transform.position, sender.transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger _colorChanger = sender.GetComponent<ColorChanger>();
        Color _currentColor = _colorChanger.DefaultColor;
        ps.startColor = _currentColor;
        deathVFX.transform.SetParent(this.transform);
    }
}
