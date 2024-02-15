#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using DG.Tweening;

[ExecuteAlways]
public class SplinePathController : MonoBehaviour
{
    [SerializeField] private Transform _pointA; // Начальная точка
    [SerializeField] private Transform _pointB; // Конечная точка
    [SerializeField, HideInInspector] private Transform[] _checkpoints; // Массив чекпоинтов

    [SerializeField] private float _duration = 5f; // Длительность анимации

    private Vector3[] _lastCheckpointPositions;

#if UNITY_EDITOR
    // Метод вызывается в редакторе Unity при изменении параметров скрипта
    private void OnValidate()
    {
        // Проверяем, что мы не находимся в режиме игры
        if (!Application.isPlaying)
        {
            // Проверяем изменения в позициях точек и чекпоинтов
            bool positionsChanged = false;

            if (_pointA != null && _pointA.hasChanged)
                positionsChanged = true;

            if (_pointB != null && _pointB.hasChanged)
                positionsChanged = true;

            if (_checkpoints != null)
            {
                for (int i = 0; i < _checkpoints.Length; i++)
                {
                    if (_checkpoints[i] != null && _checkpoints[i].hasChanged)
                    {
                        positionsChanged = true;
                        break;
                    }
                }
            }

            // Если есть изменения в позициях, перерисовываем маршрут
            if (positionsChanged)
            {
                CreateSplinePath();
                UpdateLastCheckpointPositions();
            }
        }
    }

    private void UpdateLastCheckpointPositions()
    {
        // Сохраняем текущие позиции чекпоинтов
        _lastCheckpointPositions = new Vector3[_checkpoints.Length];
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            _lastCheckpointPositions[i] = _checkpoints[i] != null ? _checkpoints[i].position : Vector3.zero;
        }
    }
#endif

    private void CreateSplinePath()
    {
        // Создаем массив точек для spline
        Vector3[] pathPoints = new Vector3[_checkpoints.Length + 2];
        pathPoints[0] = _pointA.position;

        // Заполняем массив точек чекпоинтами
        for (int i = 0; i < _checkpoints.Length; i++)
        {
            pathPoints[i + 1] = _checkpoints[i] != null ? _checkpoints[i].position : Vector3.zero;
        }

        pathPoints[pathPoints.Length - 1] = _pointB.position;

        // Используем DOTween для создания spline пути
        transform.DOPath(pathPoints, _duration, PathType.CatmullRom, PathMode.Full3D)
            .SetOptions(false)
            .SetEase(Ease.Linear)
            .OnWaypointChange(OnWaypointChange);

        // Удаление и создание Line Renderer для визуализации пути
        if (gameObject.GetComponent<LineRenderer>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<LineRenderer>());
        }
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }

    private void OnWaypointChange(int index)
    {
        Debug.Log("Достигнут чекпоинт: " + index);
    }
}
