using System;

namespace PXELDAR
{
    [Serializable]
    public class SplineBehaviour
    {
        public SplineType splineType;
        public ChunkType[] chunkType;
        public bool isLeftAndRightEnabled;
    }
}