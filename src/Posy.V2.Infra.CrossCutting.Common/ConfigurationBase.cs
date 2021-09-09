using Posy.V2.Infra.CrossCutting.Common.Enums;
using System;
using System.Configuration;

namespace Posy.V2.Infra.CrossCutting.Common
{
    public class ConfigurationBase
    {
        public static DateTime DataAtual { get; private set; } = DateTime.UtcNow;
        public static ServerDatabase ServerDatabase { get; set; } = (ServerDatabase)Convert.ToInt32(ConfigurationManager.AppSettings["ServerDatabase"]);
    }
}
