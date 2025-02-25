using System.Collections;

namespace Multimetro1_0_2.Model
{
    public class Reading_EventArgs : EventArgs
    {
        public readonly byte[] data;
        public Reading_EventArgs(byte[] data)
        {
            this.data = data;
        }



    }
    public class MeasureEventArgs : EventArgs
    {
        public readonly float value;
        public readonly ulong time;
        public MeasureEventArgs(float value, ulong time)
        {
            this.value = value;
            this.time = time;
        }
    }
    public class MeasureSignalEventArgs : EventArgs
    {
        public (float, ulong)[] Sample { get; }

        public MeasureSignalEventArgs((float, ulong)[] sample)
        {
            Sample = sample;
        }

    }
}
