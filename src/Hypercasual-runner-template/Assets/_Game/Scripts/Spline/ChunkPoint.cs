using System;

namespace PXELDAR
{
    [Serializable]
    public class ChunkPoint
    {
        //===================================================================================

        public bool isTaken;
        public double splinePercent;
        public ChunkType chunkType;

        //===================================================================================

        public ChunkPoint(bool isTaken, double splinePercent, ChunkType chunkType)
        {
            this.isTaken = isTaken;
            this.splinePercent = splinePercent;
            this.chunkType = chunkType;
        }

        //===================================================================================

    }
}