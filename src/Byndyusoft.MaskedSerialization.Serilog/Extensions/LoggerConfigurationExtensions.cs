namespace Byndyusoft.MaskedSerialization.Serilog.Extensions
{
    using global::Serilog;
    using Policies;

    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithMaskingPolicy(this LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration.Destructure.With<MaskDestructuringPolicy>();
        }
    }
}