using UnityEngine;

namespace CCN.Health
{
    public static class GizmosExtension
    {
        private static readonly Quaternion RightQuaternion = new Quaternion(0.0f, 1.0f, 0.0f, -0.3f); 
        private static readonly Quaternion LeftQuaternion = new Quaternion(0.0f, 1.0f, 0.0f, 0.3f); 
        private static readonly Quaternion UpQuaternion = new Quaternion(1.0f, 0.0f, 0.0f, -0.3f); 
        private static readonly Quaternion DownQuaternion = new Quaternion(1.0f, 0.0f, 0.0f, 0.3f);


        public static void DrawArrow(Vector3 origin, Vector3 direction, Color color)
        {
            Color old = Gizmos.color;
            Gizmos.color = color;
            DrawArrow(origin, direction);
            Gizmos.color = old;
        }

        public static void DrawArrow(Vector3 origin, Vector3 direction)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 right = lookRotation * RightQuaternion * new Vector3(0, 0, 1);
            Vector3 left = lookRotation * LeftQuaternion * new Vector3(0, 0, 1);
            Vector3 up = lookRotation * UpQuaternion * new Vector3(0, 0, 1);
            Vector3 down = lookRotation * DownQuaternion * new Vector3(0, 0, 1);

            Gizmos.DrawRay(origin, direction);
            Gizmos.DrawRay(origin + direction, right * 0.25f);
            Gizmos.DrawRay(origin + direction, left * 0.25f);
            Gizmos.DrawRay(origin + direction, up * 0.25f);
            Gizmos.DrawRay(origin + direction, down * 0.25f);
        }
        
    }
}