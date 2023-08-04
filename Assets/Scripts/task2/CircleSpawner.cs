using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public int minCircleCount = 5;
    public int maxCircleCount = 10;
    [SerializeField] private Transform[] spawnPositions;

    private void Start()
    {
        Restart();
    }

    private void SpawnCircle()
    {

        // float x = Random.Range(-6.6f, 6.6f); // Customize the spawn area as needed.
        // float y = Random.Range(-3f, 3f);
        SpriteRenderer spriteRenderer = Instantiate(circlePrefab, spawnPositions[Random.Range(0, spawnPositions.Length)].position, Quaternion.identity).GetComponent<SpriteRenderer>();
        // spriteRenderer.color = new Color(53, 45, 82, 0f);
        spriteRenderer.DOFade(1, 0.5f);

    }
    IEnumerator restart()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Circle");
        foreach (GameObject circle in circles) { Destroy(circle); }

        int circleCount = Random.Range(minCircleCount, maxCircleCount + 1);
        for (int i = 0; i < circleCount; i++)
        {
            SpawnCircle();
            yield return new WaitForSeconds(0.09f);
        }

    }
    public void Restart()
    {
        StartCoroutine("restart");
    }
}
