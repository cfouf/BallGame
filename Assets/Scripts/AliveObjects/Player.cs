using Managers;
using MetaScripts;
using UnityEngine;

namespace AliveObjects
{
    public class Player : MonoBehaviour, IMovable
    {
        private bool IsMovementLocked;
        private Rigidbody2D ComponentRigidbody;

        private int KillCount
        {
            set => guiController.UpdateScore(value);
            get => guiController.Score;
        }
        private int Health 
        {
            set => guiController.UpdateHealth(value);
            get => guiController.Health;
        }
        [SerializeField] private GUIManager guiController;
        [SerializeField] private int turnSpeed;
        [SerializeField] private int jumpForce;
        [SerializeField] private int jumpLenght;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float bottomHeight = -5;

        private void Start() =>
            ComponentRigidbody = GetComponent<Rigidbody2D>();

        private void Update()
        {
            Move();
            if (transform.position.y < bottomHeight) DestroyTheObject();
        }

        public void Move()
        {
            if (IsMovementLocked) return;
            if (Input.GetKey(KeyCode.D))
                ComponentRigidbody.AddForce(Vector2.right * turnSpeed);

            if (Input.GetKey(KeyCode.A))
                ComponentRigidbody.AddForce(Vector2.left * turnSpeed);

            if (ComponentRigidbody.velocity.magnitude > maxSpeed)
                ComponentRigidbody.velocity = ComponentRigidbody.velocity.normalized * maxSpeed;

            if (Input.GetAxisRaw("Vertical") == 0) return;
            ComponentRigidbody.AddForce(
                new Vector2(Input.GetAxisRaw("Horizontal") * jumpLenght, Input.GetAxisRaw("Vertical") * jumpForce),
                ForceMode2D.Impulse);
            IsMovementLocked = true;
        }

        private void Hurt()
        {
            Health--;
            if (Health == 0) DestroyTheObject();
        }


        public void DestroyTheObject() =>
            SceneChanger.ChangeScene(2);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 6)
            {
                KillCount++;
                Destroy(other.gameObject);
            }
            if (other.gameObject.name != "KillZone") return;
            KillCount++;
            other.transform.parent.gameObject.GetComponent<Enemy>().DestroyTheObject();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            IsMovementLocked = false;
            if (other.gameObject.name != "Enemy") return;
            Hurt();
        }
    }
}