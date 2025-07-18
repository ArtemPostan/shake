using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Receavable
{
    public float pullSpeed = 10f;
    public float pullDuration = 5f;
    public float reachDistance = 0.5f;

    private Transform playerTransform;
     
    public override void ItemUsage(Combat playerCombat)
    {
        
        playerTransform = playerCombat.transform;

        // Скрываем магнит, чтобы он "исчез"
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;

        // Запускаем притяжение
        StartCoroutine(PullDiamonds());
    }

    private IEnumerator PullDiamonds()
    {
        float timer = 0f;
        List<GameObject> diamonds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Diamond"));

        while (timer < pullDuration)
        {
            timer += Time.deltaTime;

            for (int i = diamonds.Count - 1; i >= 0; i--)
            {
                GameObject diamond = diamonds[i];
                if (diamond == null)
                {
                    diamonds.RemoveAt(i);
                    continue;
                }

                Rigidbody rb = diamond.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (playerTransform.position - diamond.transform.position).normalized;
                    rb.velocity = dir * pullSpeed;

                    // Если близко к игроку — забрать
                    if (Vector3.Distance(diamond.transform.position, playerTransform.position) < reachDistance)
                    {
                        Destroy(diamond);
                        diamonds.RemoveAt(i);
                    }
                }
            }

            if (diamonds.Count == 0)
                break;

            yield return null;
        }

        Destroy(gameObject); // Удалить магнит
    }

}