using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ResultCoinEffect : MonoBehaviour
{
    [SerializeField, Required]
    private TextMeshProUGUI text;

    public void StartEffect(int to)
    {
        DOTween.To(() => 0,
                   v => text.text = v.ToString("N0"),
                   to,
                   3f).Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
