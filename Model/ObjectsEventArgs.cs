namespace Multimetro1_0_2.Model
{
    public class Reading_EventArgs : EventArgs
    {
        public Buffer2 CurrentBuffer { get; }
        public Reading_EventArgs(string Buffer2)
        {
            CurrentBuffer = new(Buffer2);
        }
        public Reading_EventArgs(char Buffer2)
        {
            CurrentBuffer = new(Buffer2);
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
