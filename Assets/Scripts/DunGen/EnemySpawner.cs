using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace DunGen
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] Enemies;
        public GameObject Bones;
        public GameObject Lamps;
        public int MaxNumberOfEnemeies = 10;
        GameObject[] SpawnedEnemies;
        int NumberOfEnemies;
        public Text EnemyCounter;

        public static EnemySpawner Instance;

        private void Start()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }
        public void SpawnEnemies()
        {
            GameObject[] Temp = new GameObject[MaxNumberOfEnemeies];
            int count = 0;
            do
            {
                Vector3 randomLocation = Random.insideUnitSphere * 1000;
                if (NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, 10, 1))
                {

                    Temp[count] = Instantiate(Enemies[Random.Range(0, Enemies.Length)], hit.position, Quaternion.identity);
                    count++;
                }
            } while (count < MaxNumberOfEnemeies);
            SpawnedEnemies = Temp;
            NumberOfEnemies = MaxNumberOfEnemeies;
            EnemyCounter.text = "Enemies alive: " + NumberOfEnemies.ToString();
            SpawnBones();
            SpawnLamps();
        }

        void SpawnBones()
        {
            int count = 0;
            do
            {
                Vector3 randomLocation = Random.insideUnitSphere * 1000;
                if (NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, 10, 1))
                {

                    Instantiate(Lamps, hit.position, Quaternion.identity);
                    count++;
                }
            } while (count < 20);
        }

        void SpawnLamps()
        {
            int count = 0;
            do
            {
                Vector3 randomLocation = Random.insideUnitSphere * 1000;
                if (NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, 10, 1))
                {

                    Instantiate(Bones, hit.position, Quaternion.identity);
                    count++;
                }
            } while (count < 50);
        }


        public void EnemyDied()
        {
            NumberOfEnemies--;
            EnemyCounter.text = "Enemies alive: " + NumberOfEnemies.ToString();
            if (NumberOfEnemies <= 0)
            {
                // do something
                MapGenerator.Instance.NextLevel();
            }
        }

    }
}
