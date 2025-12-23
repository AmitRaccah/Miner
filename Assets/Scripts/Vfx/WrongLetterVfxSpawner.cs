using UnityEngine;

public class WrongLetterVfxSpawner : MonoBehaviour
{
    private PlayerVfxPoints cachedPoints;

    private void OnEnable()
    {
        Player.OnWrongLetterHit += HandleWrongLetterHit;
    }

    private void OnDisable()
    {
        Player.OnWrongLetterHit -= HandleWrongLetterHit;
    }

    private void HandleWrongLetterHit()
    {
        if (VfxPoolManager.Instance == null)
        {
            return;
        }

        if (cachedPoints == null)
        {
            Player playerInScene = FindObjectOfType<Player>();
            if (playerInScene == null)
            {
                return;
            }

            cachedPoints = playerInScene.GetComponent<PlayerVfxPoints>();
            if (cachedPoints == null)
            {
                return;
            }
        }

        SpawnAtPoint(cachedPoints.SpawnPointA);
        SpawnAtPoint(cachedPoints.SpawnPointB);
    }

    private void SpawnAtPoint(Transform spawnPoint)
    {
        if (spawnPoint == null)
        {
            return;
        }

        PooledParticle pooledParticle = VfxPoolManager.Instance.GetParticle();
        if (pooledParticle == null)
        {
            return;
        }

        pooledParticle.PlayAtPosition(spawnPoint.position);
    }
}
