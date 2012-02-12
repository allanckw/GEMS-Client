﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace evmsService.entities
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="User", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class User : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string EmailField;
        
        private string NameField;
        
        private bool isEventOrganizerField;
        
        private bool isLocationAdminField;
        
        private bool isNormalUserField;
        
        private bool isSystemAdminField;
        
        private string userIDField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email
        {
            get
            {
                return this.EmailField;
            }
            set
            {
                this.EmailField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isEventOrganizer
        {
            get
            {
                return this.isEventOrganizerField;
            }
            set
            {
                this.isEventOrganizerField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isLocationAdmin
        {
            get
            {
                return this.isLocationAdminField;
            }
            set
            {
                this.isLocationAdminField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isNormalUser
        {
            get
            {
                return this.isNormalUserField;
            }
            set
            {
                this.isNormalUserField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isSystemAdmin
        {
            get
            {
                return this.isSystemAdminField;
            }
            set
            {
                this.isSystemAdminField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string userID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SysRole", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class SysRole : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string RemarksField;
        
        private int RoleLevelField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Remarks
        {
            get
            {
                return this.RemarksField;
            }
            set
            {
                this.RemarksField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RoleLevel
        {
            get
            {
                return this.RoleLevelField;
            }
            set
            {
                this.RoleLevelField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Event", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Event : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string DescriptionField;
        
        private System.DateTime EndDateTimeField;
        
        private evmsService.entities.EventContact[] EventContactListField;
        
        private evmsService.entities.Guest[] GuestListField;
        
        private evmsService.entities.Item[] ItemListField;
        
        private evmsService.entities.ItemType[] ItemTypeListField;
        
        private evmsService.entities.Location[] LocationField;
        
        private evmsService.entities.ManPower[] ManPowerListField;
        
        private string NameField;
        
        private evmsService.entities.User OrganizerField;
        
        private evmsService.entities.Participant[] ParticipantListField;
        
        private evmsService.entities.Program[] ProgramsListField;
        
        private System.DateTime StartDateTimeField;
        
        private evmsService.entities.Task[] TaskListField;
        
        private string WebsiteField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime EndDateTime
        {
            get
            {
                return this.EndDateTimeField;
            }
            set
            {
                this.EndDateTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.EventContact[] EventContactList
        {
            get
            {
                return this.EventContactListField;
            }
            set
            {
                this.EventContactListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.Guest[] GuestList
        {
            get
            {
                return this.GuestListField;
            }
            set
            {
                this.GuestListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.Item[] ItemList
        {
            get
            {
                return this.ItemListField;
            }
            set
            {
                this.ItemListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.ItemType[] ItemTypeList
        {
            get
            {
                return this.ItemTypeListField;
            }
            set
            {
                this.ItemTypeListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.Location[] Location
        {
            get
            {
                return this.LocationField;
            }
            set
            {
                this.LocationField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.ManPower[] ManPowerList
        {
            get
            {
                return this.ManPowerListField;
            }
            set
            {
                this.ManPowerListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.User Organizer
        {
            get
            {
                return this.OrganizerField;
            }
            set
            {
                this.OrganizerField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.Participant[] ParticipantList
        {
            get
            {
                return this.ParticipantListField;
            }
            set
            {
                this.ParticipantListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.Program[] ProgramsList
        {
            get
            {
                return this.ProgramsListField;
            }
            set
            {
                this.ProgramsListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime StartDateTime
        {
            get
            {
                return this.StartDateTimeField;
            }
            set
            {
                this.StartDateTimeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public evmsService.entities.Task[] TaskList
        {
            get
            {
                return this.TaskListField;
            }
            set
            {
                this.TaskListField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Website
        {
            get
            {
                return this.WebsiteField;
            }
            set
            {
                this.WebsiteField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EventContact", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class EventContact : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Guest", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Guest : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Item", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Item : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ItemType", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class ItemType : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Location", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Location : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ManPower", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class ManPower : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Participant", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Participant : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Program", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Program : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Task", Namespace="http://schemas.datacontract.org/2004/07/evmsService.entities")]
    public partial class Task : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
    }
}
namespace evmsService.Controllers
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    public partial class InvalidUserException : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string ReasonField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Reason
        {
            get
            {
                return this.ReasonField;
            }
            set
            {
                this.ReasonField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IEvmsService")]
public interface IEvmsService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/login", ReplyAction="http://tempuri.org/IEvmsService/loginResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/loginInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    evmsService.entities.User login(string userid, string password);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/assignLocationAdmin", ReplyAction="http://tempuri.org/IEvmsService/assignLocationAdminResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/assignLocationAdminInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    bool assignLocationAdmin(evmsService.entities.User assigner, string userid, string description);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/assignEventOrganizer", ReplyAction="http://tempuri.org/IEvmsService/assignEventOrganizerResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/assignEventOrganizerInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    bool assignEventOrganizer(evmsService.entities.User assigner, string userid, string description);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/searchUser", ReplyAction="http://tempuri.org/IEvmsService/searchUserResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/searchUserInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    evmsService.entities.User[] searchUser(string name, string userid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/viewUserRole", ReplyAction="http://tempuri.org/IEvmsService/viewUserRoleResponse")]
    evmsService.entities.SysRole viewUserRole(string userid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/CreateEvent", ReplyAction="http://tempuri.org/IEvmsService/CreateEventResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/CreateEventInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    void CreateEvent(evmsService.entities.User u, string EventName, System.DateTime EventStartDateTime, System.DateTime EventEndDatetime, string EventDescription, string EventWebsite);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/ViewEvent", ReplyAction="http://tempuri.org/IEvmsService/ViewEventResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/ViewEventInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    evmsService.entities.Event[] ViewEvent(evmsService.entities.User u);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/EditEvent", ReplyAction="http://tempuri.org/IEvmsService/EditEventResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/EditEventInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    void EditEvent(evmsService.entities.User u, evmsService.entities.Event e);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEvmsService/DeleteEvent", ReplyAction="http://tempuri.org/IEvmsService/DeleteEventResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(evmsService.Controllers.InvalidUserException), Action="http://tempuri.org/IEvmsService/DeleteEventInvalidUserExceptionFault", Name="InvalidUserException", Namespace="http://schemas.datacontract.org/2004/07/evmsService.Controllers")]
    void DeleteEvent(evmsService.entities.User u, evmsService.entities.Event e);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IEvmsServiceChannel : IEvmsService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class EvmsServiceClient : System.ServiceModel.ClientBase<IEvmsService>, IEvmsService
{
    
    public EvmsServiceClient()
    {
    }
    
    public EvmsServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public EvmsServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public EvmsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public EvmsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public evmsService.entities.User login(string userid, string password)
    {
        return base.Channel.login(userid, password);
    }
    
    public bool assignLocationAdmin(evmsService.entities.User assigner, string userid, string description)
    {
        return base.Channel.assignLocationAdmin(assigner, userid, description);
    }
    
    public bool assignEventOrganizer(evmsService.entities.User assigner, string userid, string description)
    {
        return base.Channel.assignEventOrganizer(assigner, userid, description);
    }
    
    public evmsService.entities.User[] searchUser(string name, string userid)
    {
        return base.Channel.searchUser(name, userid);
    }
    
    public evmsService.entities.SysRole viewUserRole(string userid)
    {
        return base.Channel.viewUserRole(userid);
    }
    
    public void CreateEvent(evmsService.entities.User u, string EventName, System.DateTime EventStartDateTime, System.DateTime EventEndDatetime, string EventDescription, string EventWebsite)
    {
        base.Channel.CreateEvent(u, EventName, EventStartDateTime, EventEndDatetime, EventDescription, EventWebsite);
    }
    
    public evmsService.entities.Event[] ViewEvent(evmsService.entities.User u)
    {
        return base.Channel.ViewEvent(u);
    }
    
    public void EditEvent(evmsService.entities.User u, evmsService.entities.Event e)
    {
        base.Channel.EditEvent(u, e);
    }
    
    public void DeleteEvent(evmsService.entities.User u, evmsService.entities.Event e)
    {
        base.Channel.DeleteEvent(u, e);
    }
}
