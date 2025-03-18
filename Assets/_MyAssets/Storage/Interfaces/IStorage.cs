using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage
{
    void Save<T>(T data) where T : new();
    T Load<T>() where T : new();

    bool IsStorageEmpty(string key);
}
