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
            application.ApplicationType = Opc.Ua.ApplicationType.Client;
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
    }
}
