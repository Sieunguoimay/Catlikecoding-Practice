using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : PersistableObject
{
    [SerializeField] private ShapeFactory shapeFactory;
    [SerializeField] private KeyCode createKey;
    [SerializeField] private KeyCode newGameKey;
    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;

    private List<Shape> objects = new List<Shape>();

    private PersistentStorage storage;
    private PersistentStorage Storage => storage ?? (storage = GetComponentInChildren<PersistentStorage>());

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            Storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            Storage.Load(this);
        }
    }

    private void BeginNewGame()
    {
        foreach (var o in objects)
        {
            Destroy(o.gameObject);
        }

        objects.Clear();
    }

    private void CreateObject()
    {
        var o =shapeFactory.GetRandom();
        var t = o.transform;
        t.localPosition = UnityEngine.Random.insideUnitSphere * 5f;
        t.localRotation = UnityEngine.Random.rotation;
        t.localScale = Vector3.one * UnityEngine.Random.Range(0.1f, 1f);
        objects.Add(o);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        var count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            var shapeId = reader.ReadInt();
            var o = shapeFactory.Get(shapeId);
            o.Load(reader);
            objects.Add(o);
        }
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(objects.Count);
        foreach (var o in objects)
        {
            writer.Write(o.ShapeId);
            o.Save(writer);
        }
    }
}