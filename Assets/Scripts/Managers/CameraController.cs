using UnityEngine;

namespace Managers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float damping = 1.5f;
        [SerializeField] private Vector2 offset = new(2f, 1f);
        [SerializeField] private bool faceLeft;
        [SerializeField] private GameObject Player;
        private int lastX;

        private void Start()
        {
            offset = new Vector2(Mathf.Abs(offset.x), offset.y);
            FindPlayer(faceLeft);
        }

        private void FindPlayer(bool playerFaceLeft)
        {
            var playerPosition = Player.transform.position;
            lastX = Mathf.RoundToInt(playerPosition.x);
            transform.position = playerFaceLeft
                ? new Vector3(playerPosition.x - offset.x, playerPosition.y + offset.y,
                    transform.position.z)
                : new Vector3(playerPosition.x + offset.x, playerPosition.y + offset.y,
                    transform.position.z);
        }

        private void Update()
        {
            if (!Player) return;
            var currentX = Mathf.RoundToInt(Player.transform.position.x);
            if (currentX > lastX) faceLeft = false;
            else if (currentX < lastX) faceLeft = true;
            var playerPosition = Player.transform.position;
            lastX = Mathf.RoundToInt(playerPosition.x);

            var target = faceLeft
                ? new Vector3(playerPosition.x - offset.x, playerPosition.y + offset.y,
                    transform.position.z)
                : new Vector3(playerPosition.x + offset.x, playerPosition.y + offset.y,
                    transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
        }
    }
}