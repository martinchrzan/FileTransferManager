using System;

namespace IOExtensions
{
    public class TransferProgress
    {
        public TransferProgress(DateTime startedTimestamp, long bytesTransfered)
        {
            BytesTransferred = bytesTransfered;
            BytesPerSecond = BytesTransferred / DateTime.Now.Subtract(startedTimestamp).TotalSeconds;
        }

        public long Total { get; set; }

        public long Transferred { get; set; }

        public long BytesTransferred { get; set; }

        public long StreamSize { get; set; }

        public string ProcessedFile { get; set; }

        public double BytesPerSecond { get; }

        public double Fraction
        {
            get
            {
                return BytesTransferred / (double)Total;
            }
        }

        public double Percentage
        {
            get
            {
                return 100.0 * Fraction;
            }
        }

        public override string ToString()
        {
            return string.Format("Total: {0}, BytesTransferred: {1}, Percentage: {2}", Total, BytesTransferred, Percentage);
        }
    }
}
