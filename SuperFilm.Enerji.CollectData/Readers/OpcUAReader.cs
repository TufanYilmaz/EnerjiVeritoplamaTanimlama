using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFilm.Enerji.CollectData.Readers
{
    public sealed class OpcUAReader
    {
        SessionReconnectHandler? m_reconnectHandler;
        public Session? m_session;
        ApplicationInstance application = new ApplicationInstance();
        ApplicationConfiguration m_configuration;
        ConfiguredEndpoint endpoint;
        ReadValueIdCollection testNodesToRead = new ReadValueIdCollection();
        public OpcUAReader()
        {
            application.ApplicationType = ApplicationType.Client;
            application.ConfigSectionName = "Client";
            application.LoadApplicationConfiguration(false).Wait();
            application.CheckApplicationInstanceCertificates(false, 0).Wait();

            m_configuration = application.ApplicationConfiguration;
            m_configuration.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;
            string serverUrl = "";

            EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(m_configuration, serverUrl, true, 15000);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(m_configuration);
            endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
            m_reconnectHandler = new SessionReconnectHandler(true, 10 * 1000);
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
        public async Task<DataValueCollection> ReadNodesAsync(ReadValueIdCollection nodesToRead,CancellationToken cancellationToken)
        {
            DataValueCollection readResults = null;
            DiagnosticInfoCollection readDiagnosticInfos = null;

            //ResponseHeader readHeader = m_session.Read(null, 0, TimestampsToReturn.Neither, nodesToRead, out readResults, out readDiagnosticInfos);
            var readRes = await m_session.ReadAsync(null, 0, TimestampsToReturn.Neither, nodesToRead, cancellationToken);
           
            var readHeader = await m_session.ReadValuesAsync( nodesToRead.Select(r=>r.NodeId).ToList(), cancellationToken);

            ClientBase.ValidateResponse(readResults, nodesToRead);
            ClientBase.ValidateDiagnosticInfos(readDiagnosticInfos, nodesToRead);

            return readResults;
        }
    }
}
