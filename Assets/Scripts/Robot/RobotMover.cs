using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RobotMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 180f;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationCurve _accelerationCurve; // Кривая ускорения
    [SerializeField] private float _accelerationDuration = 1f; // Продолжительность ускорения до максимальной скорости

    private float _currentSpeed = 0f; // Текущая скорость
    private float _accelerationTimer = 0f; // Таймер для отслеживания времени ускорения
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementDirection;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Получаем ввод от игрока
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Создаем вектор направления движения на основе ввода игрока
        _movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

        RotateRobot();
    }

    void FixedUpdate()
    {
        MoveRobot();
    }

    void MoveRobot()
    {
        if (_movementDirection == Vector2.zero)
        {
            // Если направление движения равно нулю, сбрасываем скорость и таймер ускорения
            _currentSpeed = 0f;
            _accelerationTimer = 0f;
            _animator.SetFloat("speed", 0f);
            return;
        }

        // Увеличиваем таймер ускорения
        _accelerationTimer += Time.fixedDeltaTime;

        // Рассчитываем текущую скорость с использованием кривой ускорения
        float accelerationProgress = Mathf.Clamp01(_accelerationTimer / _accelerationDuration);
        float accelerationFactor = _accelerationCurve.Evaluate(accelerationProgress);
        _currentSpeed = Mathf.Lerp(0f, _moveSpeed, accelerationFactor);

        // Обновляем параметр speed в аниматоре на основе текущей скорости
        float normalizedSpeed = _currentSpeed / _moveSpeed;
        _animator.SetFloat("Speed", normalizedSpeed);

        // Перемещаем объект на текущую скорость
        Vector2 movement = _movementDirection * _currentSpeed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(_rigidbody2D.position + movement);
    }

    void RotateRobot()
    {
        if (_movementDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(_movementDirection.y, _movementDirection.x) * Mathf.Rad2Deg;

            Vector2 tankForward = transform.right;

            float angleDiff = Vector2.SignedAngle(tankForward, _movementDirection);

            if (Mathf.Abs(angleDiff) > 90f)
            {
                targetAngle += 180f;
            }

            float angle = Mathf.MoveTowardsAngle(_rigidbody2D.rotation, targetAngle, _rotationSpeed * Time.deltaTime);

            _rigidbody2D.MoveRotation(angle);
        }
    }
}
