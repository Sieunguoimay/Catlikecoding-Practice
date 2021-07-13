using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField] private Shape[] prefabs;
    [SerializeField] private bool recycle;

    private List<Shape>[] pools;

    private Scene poolScene;
    
    public Shape Get(int shapeId)
    {
        Shape shape;
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }

            var pool = pools[shapeId];
            var lastIndex = pool.Count - 1;
            if (lastIndex >= 0)
            {
                shape = pool[lastIndex];
                shape.gameObject.SetActive(true);
                pool.RemoveAt(lastIndex);
            }
            else
            {
                shape = Instantiate(prefabs[shapeId]);
                shape.ShapeId = shapeId;
                SceneManager.MoveGameObjectToScene(shape.gameObject,poolScene);
            }
        }
        else
        {
            shape = Instantiate(prefabs[shapeId]);
            shape.ShapeId = shapeId;
        }
        return shape;
    }

    public void Reclaim(Shape shapeToRecycle)
    {
        if (recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }
    public Shape GetRandom()
    {
        return Get(Random.Range(0, prefabs.Length));
    }

    private void CreatePools()
    {
        pools = new List<Shape>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<Shape>();
        }

        poolScene = SceneManager.CreateScene(name);
    }
}