using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace PiBlog.Configuations
{
    public class AppSettings
    {
        private static readonly IConfigurationRoot _config;
        static AppSettings()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json",true,true);
            _config =builder.Build();
        }

        /// <summary>
        /// Sqlite
        /// </summary>
        public static string SqliteConnectionString => _config["SqliteConnectionString"];

        /// <summary>
        /// ApiVersion
        /// </summary>
        public static string ApiVersion => _config["ApiVersion"];

        /// <summary>
        /// JWT
        /// </summary>
        public static class JWT
        {
            public static string Domain => _config["JWT:Domain"];

            public static string SecurityKey => _config["JWT:SecurityKey"];

            public static int Expires => Convert.ToInt32(_config["JWT:Expires"]);

            public static string Issuer =>_config["JWT:Issuer"];

            public static string Audience=>_config["JWT:Audience"];
        }

        /// <summary>
        /// GitHub
        /// </summary>
        public static class GitHub
        {
            public static int Id => Convert.ToInt32(_config["Github:Id"]);

            public static string Client_ID => _config["Github:ClientID"];

            public static string Client_Secret => _config["Github:ClientSecret"];

            public static string Redirect_Uri => _config["Github:RedirectUri"];

            public static string ApplicationName => _config["Github:ApplicationName"];
        }

        public static class Google{
             public static string Client_ID => _config["Google:ClientID"];

            public static string Client_Secret => _config["Google:ClientSecret"];
        }

    }
}