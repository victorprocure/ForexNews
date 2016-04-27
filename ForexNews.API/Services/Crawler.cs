using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ForexNews.API.Services
{
    public class Crawler
    {
        public bool Connected { get; private set; } = false;

        public Crawler(string url)
        {
            this.Connected = this.TestConnection(url);
        }

        public Crawler()
        {
        }

        public bool TestConnection(Uri url)
        {
            var uri = url.Host;

            return TestConnection(uri);
        }

        public bool TestConnection(string url)
        {
            Ping pinger = new Ping();

            var uri = new UriBuilder(url).Uri;
            IPAddress host;

            try
            {
                host = Dns.GetHostAddresses(uri.Host).First();
            }
            catch (SocketException)
            {
                // No DNS for url
                return false;
            }

            try
            {
                PingReply reply = pinger.Send(host);
                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Connection cannot be made
                return false;
            }
        }
    }
}