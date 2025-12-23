using System.Collections.Generic;
using UnityEngine;

public class VfxPoolManager : MonoBehaviour
{
    public static VfxPoolManager Instance { get; private set; }

    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private int initialSize = 4;
    [SerializeField] private Transform poolRoot;

    private Queue<PooledParticle> availableParticles = new Queue<PooledParticle>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        WarmUp();
    }

    private void WarmUp()
    {
        if (particlePrefab == null)
        {
            return;
        }

        for (int i = 0; i < initialSize; i++)
        {
            PooledParticle newParticle = CreateNew();
            Release(newParticle);
        }
    }

    private PooledParticle CreateNew()
    {
        ParticleSystem particleSystemInstance = Instantiate(particlePrefab, poolRoot);
        GameObject particleGameObject = particleSystemInstance.gameObject;
        particleGameObject.SetActive(false);

        PooledParticle pooledParticle = particleGameObject.GetComponent<PooledParticle>();
        if (pooledParticle == null)
        {
            pooledParticle = particleGameObject.AddComponent<PooledParticle>();
        }

        pooledParticle.Bind(this);
        return pooledParticle;
    }

    public PooledParticle GetParticle()
    {
        if (availableParticles.Count > 0)
        {
            return availableParticles.Dequeue();
        }

        if (particlePrefab == null)
        {
            return null;
        }

        return CreateNew();
    }

    public void Release(PooledParticle pooledParticle)
    {
        if (pooledParticle == null)
        {
            return;
        }

        pooledParticle.transform.SetParent(poolRoot, true);
        pooledParticle.gameObject.SetActive(false);

        availableParticles.Enqueue(pooledParticle);
    }
}
