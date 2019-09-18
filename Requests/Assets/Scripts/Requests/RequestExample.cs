using System.Linq;
using UnityEngine;

public class RequestExample : MonoBehaviour
{
    /// <summary>
    /// Время через которое отправляются запросы на получение параметров
    /// </summary>
    private float _timeRemaining = Glossary.RequestTime;

    /// <summary>
    /// Выполняет запросы к Серверу
    /// </summary>
    private RequestController _requestController;


    // Start is called before the first frame update
    void Start()
    {
        _requestController = new RequestController();
    }

    // Update is called once per frame
    void Update()
    {
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0)
        {
            // Получаем значения параметров
            var dataObjects = _requestController.CompactGet().ToList();

            // Выводим в консоль текущие значения параметров (Это потом убрать)
            Debug.Log($"Объект: {dataObjects[0].Name}, " +
                      $"параметр rightTemperature: {dataObjects[0].Properties["rightTemperature"]}, " +
                      $"параметр leftTemperature: {dataObjects[0].Properties["leftTemperature"]}");

            // Добавление времени таймеру
            _timeRemaining = Glossary.RequestTime;

            var bindingRight = new Binding("bindingRight", 1, "rightTemperature", 40, 50);
            var bindingLeft = new Binding("bindingLeft", 1, "leftTemperature", 70, 80);

            bindingRight.Transformation = new Transformation(90, 100);
            _requestController.ModifyBinding(bindingRight);

        }
    }
    
}
