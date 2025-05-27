using Opc.Ua;
using Opc.Ua.Client;
using static System.Net.Mime.MediaTypeNames;
using Opc.Ua.Configuration;
using Microsoft.Extensions.Configuration;

namespace SuperFilm.Enerji.OpcUAReader
{
    public class UAReaderClient : IUAReaderClient
    {
        SessionReconnectHandler? m_reconnectHandler;
        public Session? m_session;
        ApplicationInstance application = new ApplicationInstance();
        ApplicationConfiguration m_configuration;
        ConfiguredEndpoint endpoint;
        ReadValueIdCollection testNodesToRead = new ReadValueIdCollection();
        static UAReaderClient _instance;
        public UAReaderClient()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        public static UAReaderClient GetInstance()
        {
            return _instance;
        }
        public UAReaderClient (string url):base()
        {
            application.ApplicationType = ApplicationType.Client;
            application.LoadApplicationConfiguration(@"C:\Client.Config.xml", false);
            application.ConfigSectionName = "Client";
            application.LoadApplicationConfiguration(false).Wait();
            application.CheckApplicationInstanceCertificates(false, 0).Wait();
            m_configuration = application.ApplicationConfiguration;
            //var configuration = new ApplicationConfiguration()
            //{
            //    ApplicationName = "Superfilm Enerji Veritoplama",
            //    ApplicationUri = url,
            //    ProductUri = "http://opcfoundation.org/UA/DataAccessClient",
            //    ApplicationType = ApplicationType.Client,
            //    ClientConfiguration = new ClientConfiguration()
            //    {
            //        DefaultSessionTimeout = 60000,
            //        MinSubscriptionLifetime = 10000,
            //        WellKnownDiscoveryUrls = new StringCollection(new List<string>() { "opc.tcp://{0}:4840", "http://{0}:52601/UADiscovery", "http://{0}/UADiscovery/Default.svc" })
            //    },
            //    TransportConfigurations = new TransportConfigurationCollection(),
            //    TransportQuotas = new TransportQuotas()
            //    {
            //        OperationTimeout = 600000,
            //        MaxStringLength = 1048576,
            //        MaxByteStringLength = 1048576,
            //        MaxArrayLength = 65535,
            //        MaxMessageSize = 4194304,
            //        MaxBufferSize = 65535,
            //        ChannelLifetime = 300000,
            //        SecurityTokenLifetime = 3600000,
            //    },
            //    DiscoveryServerConfiguration = new DiscoveryServerConfiguration(),
            //    ServerConfiguration = new ServerConfiguration(),
            //    TraceConfiguration = new TraceConfiguration()
            //    {
            //        DeleteOnLoad = false,
            //        OutputFilePath = @"Logs\Quickstarts.DataAccessClient.log.txt",
            //        TraceMasks = 515
            //    },
            //    Extensions = new XmlElementCollection(),
            //    SecurityConfiguration = new SecurityConfiguration()
            //    {
            //        ApplicationCertificate = new CertificateIdentifier()
            //        {
            //            StoreType = "Directory",
            //            StorePath = @"%CommonApplicationData%\OPC Foundation\pki\own",
            //            SubjectName = "SubjectName",
            //        },
            //        TrustedIssuerCertificates = new CertificateTrustList()
            //        {
            //            StoreType = "Directory",
            //            StorePath = @"%CommonApplicationData%\OPC Foundation\pki\issuer"
            //        },
            //        TrustedPeerCertificates = new CertificateTrustList()
            //        {
            //            StoreType = "Directory",
            //            StorePath = @"%CommonApplicationData%\OPC Foundation\pki\trusted",
            //        },
            //        RejectedCertificateStore = new CertificateStoreIdentifier()
            //        {
            //            StoreType = "Directory",
            //            StorePath = @"%CommonApplicationData%\OPC Foundation\pki\rejected",
            //        },
            //    }

            //};
            //m_configuration = configuration;
            m_configuration.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;
            string serverUrl = url;
            //string serverUrl = configuration.GetValue<string>("OpcUrl")!;

            EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(m_configuration, serverUrl, true, 15000);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(m_configuration);
            endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
            m_reconnectHandler = new SessionReconnectHandler(true, 10 * 1000);
            CreateSession();
        }


        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }
        public async void CreateSession()
        {
            m_session = await Session.Create(
                m_configuration,
                endpoint,
                false,
                true,
                m_configuration.ApplicationName,
                60000,
                null,
                null);
            m_session.KeepAlive += Session_KeepAlive;
            m_reconnectHandler = new SessionReconnectHandler(true, 10 * 1000);
        }

        private void Session_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (ServiceResult.IsBad(e.Status))
            {
                m_reconnectHandler!.BeginReconnect(m_session, 1000, Server_ReconnectComplete);
            }
        }
        private void Server_ReconnectComplete(object sender, EventArgs e)
        {
            if (m_reconnectHandler!.Session != null)
            {
                if (!ReferenceEquals(m_session, m_reconnectHandler.Session))
                {
                    var session = m_session;
                    session!.KeepAlive -= Session_KeepAlive;
                    m_session = m_reconnectHandler.Session as Session;
                    m_session!.KeepAlive += Session_KeepAlive;
                    Utils.SilentDispose(session);
                }
            }
        }
        public async Task<DataValueCollection> ReadNodesAsync(ReadValueIdCollection nodesToRead, CancellationToken cancellationToken)
        {
            DataValueCollection readResults = null;
            DiagnosticInfoCollection readDiagnosticInfos = null;

            //ResponseHeader readHeader = m_session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out readResults, out readDiagnosticInfos);
            var readRes = await m_session!.ReadAsync(null, 0, TimestampsToReturn.Neither, nodesToRead, cancellationToken);

            //var readHeader = await m_session.ReadValuesAsync(nodesToRead.Select(r => r.NodeId).ToList(), cancellationToken);
            readResults = readRes.Results;
            ClientBase.ValidateResponse(readResults, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(readDiagnosticInfos, nodesToRead);

            return readResults;
        }
       
        public async Task<DataValueCollection> ReadNodesWithParameterAsync(ReadValueIdCollection nodesToRead, CancellationToken cancellationToken)
        {
            DataValueCollection readResults = null;
            DiagnosticInfoCollection readDiagnosticInfos = null;

            //ResponseHeader readHeader = m_session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out readResults, out readDiagnosticInfos);
            var readRes = await m_session!.ReadAsync(null, 0, TimestampsToReturn.Neither, nodesToRead, cancellationToken);

            var readHeader = await m_session.ReadValuesAsync(nodesToRead.Select(r => r.NodeId).ToList(), cancellationToken);
            //readRes.Results
            ClientBase.ValidateResponse(readResults, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(readDiagnosticInfos, nodesToRead);

            return readHeader.Item1;
        }

    }
}
