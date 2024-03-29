﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayrollMGT
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", ConfigurationName="PayrollMGT.PayrollManagementWebService_Port")]
    public interface PayrollManagementWebService_Port
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService:GeneratePaysl" +
            "ip", ReplyAction="*")]
        System.Threading.Tasks.Task<PayrollMGT.GeneratePayslip_Result> GeneratePayslipAsync(PayrollMGT.GeneratePayslip request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService:GenerateP9", ReplyAction="*")]
        System.Threading.Tasks.Task<PayrollMGT.GenerateP9_Result> GenerateP9Async(PayrollMGT.GenerateP9 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService:GetPayrollPer" +
            "iods", ReplyAction="*")]
        System.Threading.Tasks.Task<PayrollMGT.GetPayrollPeriods_Result> GetPayrollPeriodsAsync(PayrollMGT.GetPayrollPeriods request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService:GetPayrollYea" +
            "rs", ReplyAction="*")]
        System.Threading.Tasks.Task<PayrollMGT.GetPayrollYears_Result> GetPayrollYearsAsync(PayrollMGT.GetPayrollYears request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GeneratePayslip", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GeneratePayslip
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string employeeNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=1)]
        public string payrollPeriod;
        
        public GeneratePayslip()
        {
        }
        
        public GeneratePayslip(string employeeNo, string payrollPeriod)
        {
            this.employeeNo = employeeNo;
            this.payrollPeriod = payrollPeriod;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GeneratePayslip_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GeneratePayslip_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string return_value;
        
        public GeneratePayslip_Result()
        {
        }
        
        public GeneratePayslip_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GenerateP9", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GenerateP9
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string employeeNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=1)]
        public int year;
        
        public GenerateP9()
        {
        }
        
        public GenerateP9(string employeeNo, int year)
        {
            this.employeeNo = employeeNo;
            this.year = year;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GenerateP9_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GenerateP9_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string return_value;
        
        public GenerateP9_Result()
        {
        }
        
        public GenerateP9_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPayrollPeriods", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GetPayrollPeriods
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string employeeNo;
        
        public GetPayrollPeriods()
        {
        }
        
        public GetPayrollPeriods(string employeeNo)
        {
            this.employeeNo = employeeNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPayrollPeriods_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GetPayrollPeriods_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string return_value;
        
        public GetPayrollPeriods_Result()
        {
        }
        
        public GetPayrollPeriods_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPayrollYears", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GetPayrollYears
    {
        
        public GetPayrollYears()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPayrollYears_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", IsWrapped=true)]
    public partial class GetPayrollYears_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/PayrollManagementWebService", Order=0)]
        public string return_value;
        
        public GetPayrollYears_Result()
        {
        }
        
        public GetPayrollYears_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    public interface PayrollManagementWebService_PortChannel : PayrollMGT.PayrollManagementWebService_Port, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    public partial class PayrollManagementWebService_PortClient : System.ServiceModel.ClientBase<PayrollMGT.PayrollManagementWebService_Port>, PayrollMGT.PayrollManagementWebService_Port
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public PayrollManagementWebService_PortClient() : 
                base(PayrollManagementWebService_PortClient.GetDefaultBinding(), PayrollManagementWebService_PortClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.PayrollManagementWebService_Port.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PayrollManagementWebService_PortClient(EndpointConfiguration endpointConfiguration) : 
                base(PayrollManagementWebService_PortClient.GetBindingForEndpoint(endpointConfiguration), PayrollManagementWebService_PortClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PayrollManagementWebService_PortClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(PayrollManagementWebService_PortClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PayrollManagementWebService_PortClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(PayrollManagementWebService_PortClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PayrollManagementWebService_PortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<PayrollMGT.GeneratePayslip_Result> PayrollMGT.PayrollManagementWebService_Port.GeneratePayslipAsync(PayrollMGT.GeneratePayslip request)
        {
            return base.Channel.GeneratePayslipAsync(request);
        }
        
        public System.Threading.Tasks.Task<PayrollMGT.GeneratePayslip_Result> GeneratePayslipAsync(string employeeNo, string payrollPeriod)
        {
            PayrollMGT.GeneratePayslip inValue = new PayrollMGT.GeneratePayslip();
            inValue.employeeNo = employeeNo;
            inValue.payrollPeriod = payrollPeriod;
            return ((PayrollMGT.PayrollManagementWebService_Port)(this)).GeneratePayslipAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<PayrollMGT.GenerateP9_Result> PayrollMGT.PayrollManagementWebService_Port.GenerateP9Async(PayrollMGT.GenerateP9 request)
        {
            return base.Channel.GenerateP9Async(request);
        }
        
        public System.Threading.Tasks.Task<PayrollMGT.GenerateP9_Result> GenerateP9Async(string employeeNo, int year)
        {
            PayrollMGT.GenerateP9 inValue = new PayrollMGT.GenerateP9();
            inValue.employeeNo = employeeNo;
            inValue.year = year;
            return ((PayrollMGT.PayrollManagementWebService_Port)(this)).GenerateP9Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<PayrollMGT.GetPayrollPeriods_Result> PayrollMGT.PayrollManagementWebService_Port.GetPayrollPeriodsAsync(PayrollMGT.GetPayrollPeriods request)
        {
            return base.Channel.GetPayrollPeriodsAsync(request);
        }
        
        public System.Threading.Tasks.Task<PayrollMGT.GetPayrollPeriods_Result> GetPayrollPeriodsAsync(string employeeNo)
        {
            PayrollMGT.GetPayrollPeriods inValue = new PayrollMGT.GetPayrollPeriods();
            inValue.employeeNo = employeeNo;
            return ((PayrollMGT.PayrollManagementWebService_Port)(this)).GetPayrollPeriodsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<PayrollMGT.GetPayrollYears_Result> PayrollMGT.PayrollManagementWebService_Port.GetPayrollYearsAsync(PayrollMGT.GetPayrollYears request)
        {
            return base.Channel.GetPayrollYearsAsync(request);
        }
        
        public System.Threading.Tasks.Task<PayrollMGT.GetPayrollYears_Result> GetPayrollYearsAsync()
        {
            PayrollMGT.GetPayrollYears inValue = new PayrollMGT.GetPayrollYears();
            return ((PayrollMGT.PayrollManagementWebService_Port)(this)).GetPayrollYearsAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.PayrollManagementWebService_Port))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                result.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Basic;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.PayrollManagementWebService_Port))
            {
                return new System.ServiceModel.EndpointAddress("http://desktop-csglh1r:7347/PLATCORP/WS/Platcorp Training/Codeunit/PayrollManagem" +
                        "entWebService");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return PayrollManagementWebService_PortClient.GetBindingForEndpoint(EndpointConfiguration.PayrollManagementWebService_Port);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return PayrollManagementWebService_PortClient.GetEndpointAddress(EndpointConfiguration.PayrollManagementWebService_Port);
        }
        
        public enum EndpointConfiguration
        {
            
            PayrollManagementWebService_Port,
        }
    }
}
