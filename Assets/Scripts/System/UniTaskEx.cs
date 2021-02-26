using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class UniTaskEx
{
    public static UniTask WhenAll(this IEnumerable<UniTask> uniTasks) =>
        UniTask.WhenAll(uniTasks);
}
