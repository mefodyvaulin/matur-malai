using UnityEngine;

namespace EnemyGroup
{
    public class EnemyGroupCircle : EnemyGroupAbstract
    {
        private readonly Vector3 centerOfCircle;

        // Вращение //
        private readonly float radius;
        private float rotationSpeed = 1f; // Скорость вращения
        private const float RotationAngle = Mathf.PI / 100f;

        // Растяжение //
        private float actualRadius = 6f;
        private float minRadius = 3f;
        private float maxRadius = 8f;
        private float centerSpeed = 0.1f;
        private bool isUp = true;


        public EnemyGroupCircle(int countDrones, Vector3 spawnPosition) : base(countDrones, spawnPosition)
        {
            centerOfCircle = spawnPosition + new Vector3(15, 0, 0);
            radius = 6f;
        }

        public override Vector3 TakePosition(int index)
        {
            var angle = 2 * Mathf.PI / countDrones * index;
            return centerOfCircle + new Vector3(
                radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle),
                0
            );
        }

        public override void MoveGroup(Enemy enemy)
        {
            UpdateRadius();

            var direction = new Vector2(
                enemy.transform.position.x - centerOfCircle.x,
                enemy.transform.position.y - centerOfCircle.y
            );

            var newDirection = new Vector2(
                direction.x * Mathf.Cos(RotationAngle * rotationSpeed) - direction.y * Mathf.Sin(RotationAngle * rotationSpeed),
                direction.x * Mathf.Sin(RotationAngle * rotationSpeed) + direction.y * Mathf.Cos(RotationAngle * rotationSpeed)
            ).normalized * actualRadius;

            var positionDelta = newDirection - direction;

            enemy.transform.position += new Vector3(
                positionDelta.x,
                positionDelta.y,
                0
            );

            enemy.shooting.UpdateShooting();
        }

        private void UpdateRadius()
        {
            if (isUp)
            {
                actualRadius += centerSpeed * Time.deltaTime;
                if (actualRadius >= maxRadius)
                    isUp = false;
            }
            else
            {
                actualRadius -= centerSpeed * Time.deltaTime;
                if (actualRadius <= minRadius)
                {
                    isUp = true;
                }
            }
        }
    }
}