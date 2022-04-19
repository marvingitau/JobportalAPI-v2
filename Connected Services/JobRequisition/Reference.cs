﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobRequisition
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", ConfigurationName="JobRequisition.JRWS_Port")]
    public interface JRWS_Port
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:GetPostedJobs", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.GetPostedJobs_Result> GetPostedJobsAsync(JobRequisition.GetPostedJobs request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:GetPostedJobRequirements", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.GetPostedJobRequirements_Result> GetPostedJobRequirementsAsync(JobRequisition.GetPostedJobRequirements request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:GetPostedJobQualifications", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.GetPostedJobQualifications_Result> GetPostedJobQualificationsAsync(JobRequisition.GetPostedJobQualifications request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:GetJobTasks", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.GetJobTasks_Result> GetJobTasksAsync(JobRequisition.GetJobTasks request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:GetJobMeta", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.GetJobMeta_Result> GetJobMetaAsync(JobRequisition.GetJobMeta request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:PostJobApplication", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.PostJobApplication_Result> PostJobApplicationAsync(JobRequisition.PostJobApplication request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:JobApplicationModified", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.JobApplicationModified_Result> JobApplicationModifiedAsync(JobRequisition.JobApplicationModified request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:microsoft-dynamics-schemas/codeunit/JRWS:GetChecklist", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<JobRequisition.GetChecklist_Result> GetChecklistAsync(JobRequisition.GetChecklist request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPostedJobs", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetPostedJobs
    {
        
        public GetPostedJobs()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPostedJobs_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetPostedJobs_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public GetPostedJobs_Result()
        {
        }
        
        public GetPostedJobs_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPostedJobRequirements", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetPostedJobRequirements
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string jobNo;
        
        public GetPostedJobRequirements()
        {
        }
        
        public GetPostedJobRequirements(string jobNo)
        {
            this.jobNo = jobNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPostedJobRequirements_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetPostedJobRequirements_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public GetPostedJobRequirements_Result()
        {
        }
        
        public GetPostedJobRequirements_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPostedJobQualifications", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetPostedJobQualifications
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string jobNo;
        
        public GetPostedJobQualifications()
        {
        }
        
        public GetPostedJobQualifications(string jobNo)
        {
            this.jobNo = jobNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetPostedJobQualifications_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetPostedJobQualifications_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public GetPostedJobQualifications_Result()
        {
        }
        
        public GetPostedJobQualifications_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetJobTasks", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetJobTasks
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string jobNo;
        
        public GetJobTasks()
        {
        }
        
        public GetJobTasks(string jobNo)
        {
            this.jobNo = jobNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetJobTasks_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetJobTasks_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public GetJobTasks_Result()
        {
        }
        
        public GetJobTasks_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetJobMeta", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetJobMeta
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string reqNo;
        
        public GetJobMeta()
        {
        }
        
        public GetJobMeta(string reqNo)
        {
            this.reqNo = reqNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetJobMeta_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetJobMeta_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public GetJobMeta_Result()
        {
        }
        
        public GetJobMeta_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostJobApplication", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class PostJobApplication
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string reqNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=1)]
        public string employeeNo;
        
        public PostJobApplication()
        {
        }
        
        public PostJobApplication(string reqNo, string employeeNo)
        {
            this.reqNo = reqNo;
            this.employeeNo = employeeNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostJobApplication_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class PostJobApplication_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public PostJobApplication_Result()
        {
        }
        
        public PostJobApplication_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="JobApplicationModified", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class JobApplicationModified
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string jobAppNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute("jobAppDataText")]
        public string[] jobAppDataText;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime dOB;
        
        public JobApplicationModified()
        {
        }
        
        public JobApplicationModified(string jobAppNo, string[] jobAppDataText, System.DateTime dOB)
        {
            this.jobAppNo = jobAppNo;
            this.jobAppDataText = jobAppDataText;
            this.dOB = dOB;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="JobApplicationModified_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class JobApplicationModified_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public bool return_value;
        
        public JobApplicationModified_Result()
        {
        }
        
        public JobApplicationModified_Result(bool return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetChecklist", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetChecklist
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string jobReqNo;
        
        public GetChecklist()
        {
        }
        
        public GetChecklist(string jobReqNo)
        {
            this.jobReqNo = jobReqNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetChecklist_Result", WrapperNamespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", IsWrapped=true)]
    public partial class GetChecklist_Result
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:microsoft-dynamics-schemas/codeunit/JRWS", Order=0)]
        public string return_value;
        
        public GetChecklist_Result()
        {
        }
        
        public GetChecklist_Result(string return_value)
        {
            this.return_value = return_value;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    public interface JRWS_PortChannel : JobRequisition.JRWS_Port, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    public partial class JRWS_PortClient : System.ServiceModel.ClientBase<JobRequisition.JRWS_Port>, JobRequisition.JRWS_Port
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public JRWS_PortClient() : 
                base(JRWS_PortClient.GetDefaultBinding(), JRWS_PortClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.JRWS_Port.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JRWS_PortClient(EndpointConfiguration endpointConfiguration) : 
                base(JRWS_PortClient.GetBindingForEndpoint(endpointConfiguration), JRWS_PortClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JRWS_PortClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(JRWS_PortClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JRWS_PortClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(JRWS_PortClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public JRWS_PortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.GetPostedJobs_Result> JobRequisition.JRWS_Port.GetPostedJobsAsync(JobRequisition.GetPostedJobs request)
        {
            return base.Channel.GetPostedJobsAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.GetPostedJobs_Result> GetPostedJobsAsync()
        {
            JobRequisition.GetPostedJobs inValue = new JobRequisition.GetPostedJobs();
            return ((JobRequisition.JRWS_Port)(this)).GetPostedJobsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.GetPostedJobRequirements_Result> JobRequisition.JRWS_Port.GetPostedJobRequirementsAsync(JobRequisition.GetPostedJobRequirements request)
        {
            return base.Channel.GetPostedJobRequirementsAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.GetPostedJobRequirements_Result> GetPostedJobRequirementsAsync(string jobNo)
        {
            JobRequisition.GetPostedJobRequirements inValue = new JobRequisition.GetPostedJobRequirements();
            inValue.jobNo = jobNo;
            return ((JobRequisition.JRWS_Port)(this)).GetPostedJobRequirementsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.GetPostedJobQualifications_Result> JobRequisition.JRWS_Port.GetPostedJobQualificationsAsync(JobRequisition.GetPostedJobQualifications request)
        {
            return base.Channel.GetPostedJobQualificationsAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.GetPostedJobQualifications_Result> GetPostedJobQualificationsAsync(string jobNo)
        {
            JobRequisition.GetPostedJobQualifications inValue = new JobRequisition.GetPostedJobQualifications();
            inValue.jobNo = jobNo;
            return ((JobRequisition.JRWS_Port)(this)).GetPostedJobQualificationsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.GetJobTasks_Result> JobRequisition.JRWS_Port.GetJobTasksAsync(JobRequisition.GetJobTasks request)
        {
            return base.Channel.GetJobTasksAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.GetJobTasks_Result> GetJobTasksAsync(string jobNo)
        {
            JobRequisition.GetJobTasks inValue = new JobRequisition.GetJobTasks();
            inValue.jobNo = jobNo;
            return ((JobRequisition.JRWS_Port)(this)).GetJobTasksAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.GetJobMeta_Result> JobRequisition.JRWS_Port.GetJobMetaAsync(JobRequisition.GetJobMeta request)
        {
            return base.Channel.GetJobMetaAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.GetJobMeta_Result> GetJobMetaAsync(string reqNo)
        {
            JobRequisition.GetJobMeta inValue = new JobRequisition.GetJobMeta();
            inValue.reqNo = reqNo;
            return ((JobRequisition.JRWS_Port)(this)).GetJobMetaAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.PostJobApplication_Result> JobRequisition.JRWS_Port.PostJobApplicationAsync(JobRequisition.PostJobApplication request)
        {
            return base.Channel.PostJobApplicationAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.PostJobApplication_Result> PostJobApplicationAsync(string reqNo, string employeeNo)
        {
            JobRequisition.PostJobApplication inValue = new JobRequisition.PostJobApplication();
            inValue.reqNo = reqNo;
            inValue.employeeNo = employeeNo;
            return ((JobRequisition.JRWS_Port)(this)).PostJobApplicationAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.JobApplicationModified_Result> JobRequisition.JRWS_Port.JobApplicationModifiedAsync(JobRequisition.JobApplicationModified request)
        {
            return base.Channel.JobApplicationModifiedAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.JobApplicationModified_Result> JobApplicationModifiedAsync(string jobAppNo, string[] jobAppDataText, System.DateTime dOB)
        {
            JobRequisition.JobApplicationModified inValue = new JobRequisition.JobApplicationModified();
            inValue.jobAppNo = jobAppNo;
            inValue.jobAppDataText = jobAppDataText;
            inValue.dOB = dOB;
            return ((JobRequisition.JRWS_Port)(this)).JobApplicationModifiedAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<JobRequisition.GetChecklist_Result> JobRequisition.JRWS_Port.GetChecklistAsync(JobRequisition.GetChecklist request)
        {
            return base.Channel.GetChecklistAsync(request);
        }
        
        public System.Threading.Tasks.Task<JobRequisition.GetChecklist_Result> GetChecklistAsync(string jobReqNo)
        {
            JobRequisition.GetChecklist inValue = new JobRequisition.GetChecklist();
            inValue.jobReqNo = jobReqNo;
            return ((JobRequisition.JRWS_Port)(this)).GetChecklistAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.JRWS_Port))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.JRWS_Port))
            {
                return new System.ServiceModel.EndpointAddress("http://desktop-csglh1r:7347/PLATCORP/WS/Platcorp Training/Codeunit/JRWS");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return JRWS_PortClient.GetBindingForEndpoint(EndpointConfiguration.JRWS_Port);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return JRWS_PortClient.GetEndpointAddress(EndpointConfiguration.JRWS_Port);
        }
        
        public enum EndpointConfiguration
        {
            
            JRWS_Port,
        }
    }
}
