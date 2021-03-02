using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

public class GameEndUIHider : MonoBehaviour
{
    [Inject]
    private IObservableTimeInventory observableTimeInventory;
    [Inject]
    private IObservableCoinInventory coin;

    [SerializeField, Required]
    private Canvas appearCanvas;
    [SerializeField, Required]
    private Canvas hideCanvas;
    [SerializeField, Required]
    private ResultCoinEffect ResultCoinEffect;

    // Start is called before the first frame update
    void Start()
    {
        observableTimeInventory.Time
                               .Where(time => time <= 0)
                               .Take(1)
                               .Subscribe(_ =>
                               {
                                   hideCanvas.gameObject.SetActive(false);
                                   appearCanvas.gameObject.SetActive(true);
                                   ResultCoinEffect.StartEffect(coin.Coin.ToReactiveProperty().Value);
                               });
    }
}
