using System.Collections.Generic;
using System.Linq;
using Enemies;
using Managers;
using Missiles;
using UnityEngine;

namespace Defendable
{
    public class MissileLauncher : CannonDefense
    {
        private List<EnemyTimePositionInfo> enemyPositions = new List<EnemyTimePositionInfo>();
        protected override void Attack(Enemy enemy)
        {
            var bullet = PoolManager.Instance.GetFromPool<LaunchableMissile>(PoolObjectType.LaunchableMissile);
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = tower.rotation;
            var enemyDistance = (enemyPositions.Last().Position - enemyPositions.First().Position).normalized;
            var time = (enemyPositions.Last().Timestamp - enemyPositions.First().Timestamp) * Time.deltaTime;
            var enemySpeed = (enemyDistance * time);
            var approxPos = EnemyTimePositionInfo.CalculateInterceptionPoint3D(bullet.transform.position, bullet.Speed, enemy.Position, enemySpeed);
            if (approxPos == Vector3.zero)
                bullet.Launch(enemy.Position);
            else
                bullet.Launch(approxPos);
            LastAttackTime = Time.time;
        }

        protected override bool RotateToEnemy(Enemy enemy)
        {
            if (enemyPositions.Count == 0)
                enemyPositions.Add(new EnemyTimePositionInfo(Time.time, enemy.Position));
            var direction = (enemy.transform.position - tower.transform.position);
            var targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), tower.up);
            tower.rotation = Quaternion.RotateTowards(tower.rotation, targetRotation, SO.RotationSpeed * Time.deltaTime);


            if (Quaternion.Angle(targetRotation, tower.rotation) <= angleThreshold)
            {
                enemyPositions.Add(new EnemyTimePositionInfo(Time.time, enemy.Position));
                return true;
            }
            return false;
        }
    }
}


public class EnemyTimePositionInfo
{
    public float Timestamp { get; private set; }
    public Vector3 Position { get; private set; }

    public EnemyTimePositionInfo(float time, Vector3 position)
    {
        Timestamp = time;
        Position = position;
    }

    public static Vector3 CalculateInterceptionPoint3D(Vector3 PC, float SC, Vector3 PR, Vector3 VR)
    {
        //! Distance between turret and target
        Vector3 D = PC - PR;

        //! Scale of distance vector
        float d = D.magnitude;

        //! Speed of target scale of VR
        float SR = VR.magnitude;

        //% Quadratic EQUATION members = (ax)^2 + bx + c = 0

        float a = Mathf.Pow(SC, 2) - Mathf.Pow(SR, 2);

        float b = 2 * Vector3.Dot(D, VR);

        float c = -Vector3.Dot(D, D);

        if ((Mathf.Pow(b, 2) - (4 * (a * c))) < 0) //% The QUADRATIC FORMULA will not return a real number because sqrt(-value) is not a real number thus no interception
        {
            return Vector2.zero;//TODO: HERE, PREVENT TURRET FROM FIRING LASERS INSTEAD OF MAKING LASERS FIRE AT ZERO!
        }
        //% Quadratic FORMULA = x = (  -b+sqrt( ((b)^2) * 4*a*c )  ) / 2a
        float t = (-(b) + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * (a * c)))) / (2 * a);//% x = time to reach interception point w$$anonymous$$ch is = t

        //% Calculate point of interception as vector from calculating distance between target and interception by t * VelocityVector
        return ((t * VR) + PR);
    }
}