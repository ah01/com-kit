namespace ComKit.Core
{
    public class SerialPortBasicInfo
    {
        private int number = -1;

        public string Name { get; internal set; }

        public int Number
        {
            get
            {
                if (number == -1 && !string.IsNullOrEmpty(Name))
                {
                    int.TryParse(Name.Substring(3), out number);
                }
                return number;
            }
        }

        public bool IsAvailable()
        {
            return SerialPortHandler.IsAvailable(Name);
        }
    }
}