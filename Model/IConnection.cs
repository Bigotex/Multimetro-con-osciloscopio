namespace Multimetro1_0_2.Model
{
    public partial interface IConnection
    {
        public event EventHandler<Reading_EventArgs>? DataSerial_Received;
        public int BaudRate { get; set; }
        public string Id { get; set; }

        public int BytesToRead();
        public bool Connect();
        public void Write(string dataOut);
        public void Write(int dataOut);
        public void Write(byte[] dataOut);
        public char ReadChar();
        public void DiscardInBuffer();
        public byte[] ReadBuffer(int position = 0, int nbytes = -1);
    }
}
