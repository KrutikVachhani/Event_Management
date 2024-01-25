namespace Event_Management.BAL
{
    public static class CommonVariables
    {
        //Provides access to the current
        //Microsoft.AspNetCore.Http.IHttpContextAccessor.HttpContext
        private static IHttpContextAccessor _HttpContextAccessor;
        static CommonVariables()
        {
            _HttpContextAccessor = new HttpContextAccessor();
        }
        public static int? UserID()
        {
            return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("UserID"));
        }

        public static string UserName()
        {
            return _HttpContextAccessor.HttpContext.Session.GetString("UserName");
        }

        public static string FirstName()
        {
            return _HttpContextAccessor.HttpContext.Session.GetString("FirstName");
        }
    }
}