using UnityEngine;

public class ParticleController : SingletonMonoBehavior<ParticleController>
{

    [SerializeField] ParticleSystem destroyEffectPrefab;

    public void PlayDestroyEffect(Vector3 position)
    {
        ParticleSystem ps = Instantiate(
            destroyEffectPrefab,
            position,
            Quaternion.identity
        );

        ps.Play();

        Destroy(ps.gameObject,
            ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
