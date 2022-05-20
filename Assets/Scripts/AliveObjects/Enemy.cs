using System;
using UnityEngine;

namespace AliveObjects
{
    public class Enemy : MonoBehaviour, IMovable
    {
        private GameObject Player;
        private bool IsMovementLocked;
        private Rigidbody2D ComponentRigidbody;
        
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private float playerViewDistance;
        [SerializeField] private int speed;
        [SerializeField] private int jumpForce;
        [SerializeField] private int jumpLenght;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float bottomHeight = -10;


        private void Start()
        {
            Player = GameObject.Find("Player");
            ComponentRigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other) => IsMovementLocked = false;

        public void Move()
        {
            if (Math.Abs(Vector2.Distance(Player.transform.position, transform.position)) > playerViewDistance) return;

            if (Player.transform.position.x <= transform.position.x)
                Move(Player.transform.position.y > transform.position.y ? Directions.UpLeft : Directions.Left);

            if (Player.transform.position.x > transform.position.x)
                Move(Player.transform.position.y > transform.position.y ? Directions.UpRight : Directions.Right);
        }

        public void DestroyTheObject()
        {
            particles.transform.position = transform.position;
            particles.Play();
            Destroy(gameObject);
        }

        private void Update()
        {
            Move();
            if (transform.position.y < bottomHeight) DestroyTheObject();
        }

        private void Move(Directions direction)
        {
            if (IsMovementLocked) return;
            switch (direction)
            {
                case Directions.Right:
                    ComponentRigidbody.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                    break;
                case Directions.Left:;
                    ComponentRigidbody.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
                    break;
            }

            if (ComponentRigidbody.velocity.magnitude > maxSpeed)
                ComponentRigidbody.velocity = ComponentRigidbody.velocity.normalized * maxSpeed;

            switch (direction)
            {
                case Directions.UpLeft:
                    ComponentRigidbody.AddForce(new Vector2(-jumpLenght, jumpForce), ForceMode2D.Impulse);
                    IsMovementLocked = true;
                    break;
                case Directions.UpRight:
                    ComponentRigidbody.AddForce(new Vector2(jumpLenght, jumpForce), ForceMode2D.Impulse);
                    IsMovementLocked = true;
                    break;
            }
        }
    }
}