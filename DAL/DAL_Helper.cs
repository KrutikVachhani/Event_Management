namespace Event_Management.DAL
{
    public class DAL_Helper
    {
        public static string connectionstr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");
    }
}
