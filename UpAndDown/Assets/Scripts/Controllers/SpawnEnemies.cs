using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    //Int
    public int orbAmount;
    public int enemyCount;
    public int orbsWaves;
    private const int Value = 10;
    
    //Components
    public GameObject[] enemies;
    public GameObject orb;
    
    private void Update()
    {
        orbAmount = FindObjectsOfType<OrbMovement>().Length;
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount <= 0 || RandomValue(0, 501) == Value)
        {
            SpawnEnemy(RandomValue(1, 4));
        }
            

        if (orbAmount == 0)
        {
            SpawnOrbs(RandomValue(20, 50));
            orbsWaves++;
        }
            
    }

    private static int RandomValue(int minValue, int maxValue)
    {
        var random = Random.Range(minValue, maxValue);
        return random;
    }

    private void SpawnOrbs(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            Instantiate(orb, RandomPos(20f, 40f, -1f, 25f), Quaternion.identity);
        }
    }

    private void SpawnEnemy(int enemyAmount)
    {
        for (var i = 0; i < enemyAmount; i++)
        {
            var index = Random.Range(0, enemies.Length);
            Instantiate(enemies[index], RandomPos(25f, 35f, 20f, 25f), Quaternion.identity);
        }
    }
    
    private static Vector2 RandomPos(float minXValue, float maxXValue, float minYValue, float maxYValue)
    {
        var randomXValue = Random.Range(minXValue, maxXValue);
        var randomYValue = Random.Range(minYValue, maxYValue);
        var randomPos = new Vector2(randomXValue, randomYValue);
        return randomPos;
    }
}
