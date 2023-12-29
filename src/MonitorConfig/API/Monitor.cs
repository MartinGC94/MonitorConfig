namespace MartinGC94.MonitorConfig.API
{
    public abstract class Monitor
    {
        public LogicalDisplay LogicalDisplay { get; }
        public string FriendlyName { get; }
        public string InstanceName { get; }

        protected Monitor(LogicalDisplay logicalDisplay, string friendlyName, string instanceName)
        {
            LogicalDisplay = logicalDisplay;
            FriendlyName = friendlyName;
            InstanceName = instanceName;
        }

        public abstract uint GetBrightnessLevel();
        public abstract void SetBrightnessLevel(uint level);
        public abstract BrightnessInfo GetBrightnessInfo();

        public override string ToString()
        {
            return InstanceName;
        }
    }
}
