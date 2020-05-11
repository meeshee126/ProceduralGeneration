using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public Vector3 offset;
    public float radius;
    public LayerMask mask;
    public float spacing;
    public int spawnMin;
    public int spawnMax;
    public List<GameObject> objects;

    public void Start()
    {
        SpawnObject();
    }

    /// <summary>
    /// Spawns GameObjects in a given offset
    /// </summary>
    /// <param name="generator"></param>
    public void SpawnObject()
    {
        int dropCount = Random.Range(spawnMin, spawnMax);

        Vector3 spawnPosition = new Vector3();

        bool canSpawnHere = false;

        int catcher = 0;

        // spawns between an offset area
        for (int i = 0; i < dropCount; i++)
        {
            do
            {
                spawnPosition = this.transform.position + new Vector3(Random.Range(-offset.x / 2, offset.x / 2),
                                                                      Random.Range(-offset.y / 2, offset.y / 2),
                                                                      Random.Range(-offset.z / 2, offset.z / 2));

                canSpawnHere = PreventSpawnOverlap(spawnPosition);

                //spawn gameobject when not overlaping with other 
                if (canSpawnHere)
                {
                    GameObject newObject = Instantiate(objects[Random.Range(0, objects.Count)], spawnPosition,
                                                       Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0), transform) as GameObject;
                    break;
                }
                catcher++;

                //prevent while loop crash
                if (catcher > 500)
                {
                    Debug.Log("Too many attempts");
                    break;
                }
            } while (true);
        }
    }

    /// <summary>
    /// Avoids overlaping instantiates
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="generator"></param>
    /// <returns></returns>
    bool PreventSpawnOverlap(Vector3 spawnPosition)
    {
        Collider[] colliders  = Physics.OverlapSphere(this.transform.position, radius, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            //Get all bounds in colliding area
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float height = colliders[i].bounds.extents.y;
            float thickness = colliders[i].bounds.extents.z;

            float leftExtend = centerPoint.x - width - spacing;
            float rightExtend = centerPoint.x + width + spacing;
            float lowerExtend = centerPoint.y - height - spacing;
            float upperExtend = centerPoint.y + height + spacing;
            float forwardExtend = centerPoint.z - thickness - spacing;
            float backwardExtend = centerPoint.z + thickness + spacing;

            //Check overlaping
            if (spawnPosition.x >= leftExtend && spawnPosition.x <= rightExtend)
            {
                if (spawnPosition.y >= lowerExtend && spawnPosition.y <= upperExtend)
                {
                    if(spawnPosition.z >= forwardExtend && spawnPosition.z <= backwardExtend)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(this.transform.position, offset);
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
