﻿using System.Configuration;

namespace Posy.V2.Infra.CrossCutting.Common.Cache.Configuration
{
    public class RedisConfigurationSection : ConfigurationSection
    {
        #region Constants

        private const string HostAttributeName = "host";
        private const string PortAttributeName = "port";
        private const string PasswordAttributeName = "password";
        private const string DatabaseIDAttributeName = "databaseID";

        #endregion

        #region Properties

        [ConfigurationProperty(HostAttributeName, IsRequired = true)]
        public string Host
        {
            get { return this[HostAttributeName].ToString(); }
        }

        [ConfigurationProperty(PortAttributeName, IsRequired = true)]
        public int Port
        {
            get { return (int)this[PortAttributeName]; }
        }

        [ConfigurationProperty(PasswordAttributeName, IsRequired = false)]
        public string Password
        {
            get { return this[PasswordAttributeName].ToString(); }
        }

        [ConfigurationProperty(DatabaseIDAttributeName, IsRequired = false)]
        public long DatabaseID
        {
            get { return (long)this[DatabaseIDAttributeName]; }
        }

        #endregion
    }
}
