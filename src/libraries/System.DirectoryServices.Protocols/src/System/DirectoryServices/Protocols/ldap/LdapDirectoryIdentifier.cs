// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.DirectoryServices.Protocols
{
    public class LdapDirectoryIdentifier : DirectoryIdentifier
    {
        private readonly string[] _servers;

        public LdapDirectoryIdentifier(string server) : this(server != null ? new string[] { server } : null, false, false)
        {
        }

        public LdapDirectoryIdentifier(string server, int portNumber) : this(server != null ? new string[] { server } : null, portNumber, false, false)
        {
        }

        public LdapDirectoryIdentifier(string server, bool fullyQualifiedDnsHostName, bool connectionless) : this(server != null ? new string[] { server } : null, fullyQualifiedDnsHostName, connectionless)
        {
        }

        public LdapDirectoryIdentifier(string server, int portNumber, bool fullyQualifiedDnsHostName, bool connectionless) : this(server != null ? new string[] { server } : null, portNumber, fullyQualifiedDnsHostName, connectionless)
        {
        }

        public LdapDirectoryIdentifier(string[] servers, bool fullyQualifiedDnsHostName, bool connectionless)
        {
            // validate the servers, we don't allow space in the server name
            if (servers != null)
            {
                _servers = new string[servers.Length];
                for (int i = 0; i < servers.Length; i++)
                {
                    if (servers[i] != null)
                    {
                        string trimmedName = servers[i].Trim();
                        string[] result = trimmedName.Split(' ');
                        if (result.Length > 1)
                        {
                            throw new ArgumentException(SR.WhiteSpaceServerName);
                        }

                        _servers[i] = trimmedName;
                    }
                }
            }

            FullyQualifiedDnsHostName = fullyQualifiedDnsHostName;
            Connectionless = connectionless;
        }

        public LdapDirectoryIdentifier(string[] servers, int portNumber, bool fullyQualifiedDnsHostName, bool connectionless) : this(servers, fullyQualifiedDnsHostName, connectionless)
        {
            PortNumber = portNumber;
        }

        public string[] Servers => (_servers == null) ? Array.Empty<string>() : (string[])_servers.Clone();

        public bool Connectionless { get; }

        public bool FullyQualifiedDnsHostName { get; }

        public int PortNumber { get; } = 389;
    }
}
