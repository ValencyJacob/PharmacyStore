namespace PharmacyStore.API.Services.IServices
{
    public interface ILoggerService
    {
        //Dont forget change prop in nlog.config (Copy to Output Directory: Copy always) or log file won't created
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
