using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : Singleton<CoroutineRunner>
{
    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
