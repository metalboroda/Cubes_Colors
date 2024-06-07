using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CubeSpawner : MonoBehaviour
  {
    [SerializeField] private float _spawnRadius = 2f;
    [SerializeField] private float _minPreviousSpawn = 0.35f;
    [Header("")]
    [SerializeField] private GameObject[] _cubePrefabs;

    private List<Vector3> _spawnedPositions = new List<Vector3>();

    private void Start()
    {
      SpawnAllCubes();
    }

    private void SpawnAllCubes()
    {
      foreach (GameObject cubePrefab in _cubePrefabs)
      {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(-360, 360), 0);

        Instantiate(cubePrefab, spawnPosition, randomRotation);

        _spawnedPositions.Add(spawnPosition);
      }
    }

    private Vector3 GetRandomSpawnPosition()
    {
      Vector3 randomPosition;
      bool isPositionValid;

      do
      {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(_minPreviousSpawn, _spawnRadius);

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        randomPosition = new Vector3(x, 0, z) + transform.position;
        randomPosition.y = transform.position.y;

        isPositionValid = true;

        foreach (Vector3 spawnedPosition in _spawnedPositions)
        {
          if (Vector3.Distance(randomPosition, spawnedPosition) < _minPreviousSpawn)
          {
            isPositionValid = false;

            break;
          }
        }

      } while (isPositionValid == false);

      return randomPosition;
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
  }
}