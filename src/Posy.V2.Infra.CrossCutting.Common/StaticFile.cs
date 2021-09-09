using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Posy.V2.Infra.CrossCutting.Common
{
    /// <summary>
    /// https://madskristensen.net/blog/cache-busting-in-aspnet
    /// </summary>
    public static class StaticFile
    {
        public static string Version(string rootRelativePath)
        {
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                var absolutePath = HostingEnvironment.MapPath("~" + rootRelativePath);

                var lastChangedDateTime = File.GetLastWriteTime(absolutePath);
                var versionedUrl = rootRelativePath + "?v=" + lastChangedDateTime.Ticks;

                HttpRuntime.Cache.Insert(rootRelativePath, versionedUrl, new CacheDependency(absolutePath));
            }

            return HttpRuntime.Cache[rootRelativePath] as string;
        }
    }
}
