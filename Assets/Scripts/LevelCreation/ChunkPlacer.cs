using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelCreation
{
    public class ChunkPlacer : MonoBehaviour
    {
        [SerializeField] private Chunk[] chunks;
        [SerializeField] private Transform playerPosition;
        [SerializeField] private Chunk firstChunk;
        [SerializeField] private int maxChunkOnScene;

        private readonly List<Chunk> SpawnedChunks = new();

        private void Start() =>
            SpawnedChunks.Add(firstChunk);

        private void Update()
        {
            if (playerPosition.position.x > SpawnedChunks[^1].endPosition.position.x - 50)
                SpawnChunk();
        }

        private void SpawnChunk()
        {
            var newChunk = Instantiate(GetRandomChunk());
            var chunkPosition = SpawnedChunks[^1].endPosition.position - newChunk.startPosition.localPosition;
            newChunk.transform.position = new Vector2(chunkPosition.x,
                chunkPosition.y + Random.Range(-2, 2));
            SpawnedChunks.Add(newChunk);

            if (SpawnedChunks.Count < maxChunkOnScene) return;
            Destroy(SpawnedChunks[0].gameObject);
            SpawnedChunks.RemoveAt(0);
        }

        private Chunk GetRandomChunk()
        {
            var chances = chunks
                .Select(t => t.chanceFromDistance
                    .Evaluate(playerPosition.transform.position.x))
                .ToList();

            var value = Random.Range(0, chances.Sum());
            float sum = 0;

            for (var i = 0; i < chances.Count; i++)
            {
                sum += chances[i];
                if (value < sum)
                    return chunks[i];
            }

            return chunks[^1];
        }
    }
}