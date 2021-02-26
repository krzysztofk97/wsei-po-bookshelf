using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BookshelfLib
{
    public static class Helper
    {
        public static string GetDefaultDBConnectionString() => ConfigurationManager.ConnectionStrings["BookshelfDatabase"].ConnectionString;
    }
}
