﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyCouch.Net;

namespace MyCouch.UnitTests.Fakes
{
    public class ServerClientConnectionFake : IServerConnection
    {
        public Uri Address { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public Action<HttpRequest> BeforeSend { set; private get; }
        public Action<HttpResponseMessage> AfterSend { set; private get; }

        public ServerClientConnectionFake(ConnectionInfo connectionInfo)
        {
            Address = connectionInfo.ServerAddress;
            Timeout = connectionInfo.Timeout ?? Timeout;
        }

        public void Dispose() { }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return null;
        }
    }

    public class DbClientConnectionFake : IDbConnection
    {
        public string DbName { get; private set; }
        public Uri Address { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public Action<HttpRequest> BeforeSend { set; private get; }
        public Action<HttpResponseMessage> AfterSend { set; private get; }

        public DbClientConnectionFake(DbConnectionInfo connectionInfo)
        {
            Address = connectionInfo.ServerAddress;
            DbName = connectionInfo.DbName;
            Timeout = connectionInfo.Timeout ?? Timeout;
        }

        public void Dispose() { }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption)
        {
            return null;
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequest httpRequest, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
