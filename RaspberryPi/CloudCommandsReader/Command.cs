namespace CloudCommandsReader
{
    public class Command
    {
        public int[] Switches { get; set; }
        public int? Switch { get; set; }

        public SwitchState State { get; set; }

        public string Comment { get; set; }
    }
}