namespace Data
{
    public class DataAccessLayer
    {
        private static DataAccessLayer _instance;
        
        private static object _instanceLock = new object();

        public static DataAccessLayer Instance
        {
            get {
                if (_instance is null)
                {
                    lock (_instanceLock)
                    {
                        _instance = new DataAccessLayer();
                        return _instance;
                    }                    
                } 
                return _instance;
            }           
        }

        public FileSystemModel.Config Configuration { get; set; } = null!;

        public DataAccessLayer()
        {
            Configuration = new FileSystemModel.Config();
        }
    }
}