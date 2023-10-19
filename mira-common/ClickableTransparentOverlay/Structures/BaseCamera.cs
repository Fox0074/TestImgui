using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Common.Structures
{
    public class BaseCamera : MemoryObject
    {
        public Matrix4x4 CameraMatrix;
        public KernalInterface _driver;

        public virtual Vector3 WorldToScreen(Vector3 _Enemy, int Width = 1920, int Height = 1080)
        {
            // vector.y - this is vector.z
            var screen = new Vector3(0, 0, 0);
            Matrix4x4 temp = Matrix4x4.Transpose(CameraMatrix);

            Vector3 translationVector = new Vector3(temp.M41, temp.M43, temp.M42);
            Vector3 up = new Vector3(temp.M21, temp.M23, temp.M22);
            Vector3 right = new Vector3(temp.M11, temp.M13, temp.M12);

            float w = Vector3.Dot(_Enemy, translationVector) + temp.M44;

            if (w < 0.098f)
                return Vector3.Zero;

            float y = Vector3.Dot(_Enemy, up) + temp.M24;
            float x = Vector3.Dot(_Enemy, right) + temp.M14;

            screen.X = (Width / 2) * (1f + x / w);
            screen.Y = (Height / 2) * (1f - y / w);
            screen.Z = w;

            return screen;
        }
    }
}
