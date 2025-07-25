using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Vector3 _shift;
    [SerializeField] private float _inertiaRadius = 3f;
    [SerializeField] private float _followSpeed = 1f;
    private Vector3 targetPosition;
    private Transform _playerTransform;

    [Inject]
    public void Construct(PlayerController player)
    {
        _playerTransform = player.transform;
        targetPosition = _playerTransform.position;
    }

    private void LateUpdate()
    {
        if (_playerTransform == null) return;

        Vector3 cameraPosition = transform.position;
        Vector3 playerPosition = _playerTransform.position+_shift;
        Vector3 flatCameraPos = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        Vector3 flatPlayerPos = new Vector3(playerPosition.x, playerPosition.y, 0);
        float distance = Vector2.Distance(flatCameraPos, flatPlayerPos);
        if (distance > _inertiaRadius)
        {
            targetPosition = flatPlayerPos;
        }
            Vector3 smoothPos = Vector3.Lerp(flatCameraPos, targetPosition, _followSpeed * Time.deltaTime);
            transform.position = new Vector3(smoothPos.x, smoothPos.y, cameraPosition.z);        
    }
}