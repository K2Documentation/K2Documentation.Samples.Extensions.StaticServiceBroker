using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//import the SourceCode SDK namespaces
using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using Attributes = SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Documentation.Samples.Extensions.StaticServiceBroker.TimeZone
{
    class CustomTimeZoneServiceBroker : ServiceAssemblyBase
    {
        #region Service Broker Implementation Methods (Override base class methods)

        #region override string GetConfigSection()
        /// <summary>
        /// Sets up the required configuration items (and default values) for a Service Instance of the Service Broker. 
        /// When a new service instance is registered for this ServiceBroker, the configuration parameters are surfaced to the UI. 
        /// The configuration values are usually provided by the person registering the service instance.
        /// you can set default values for the configuration settings and indicate whether the setting is required or optional
        /// </summary>
        /// <returns>A string containing the configuration XML.</returns>
        public override string GetConfigSection()
        {
            try
            {
                //add a configuration setting to indicate whether or not we need to cater for DST in time calculations
                //Note: you can add more configuration settings by Adding more items to the ServiceConfiguration collection, like this: 
                //this.Service.ServiceConfiguration.Add("SettingName", true, string.Empty);
                //for this particular Broker, we have a mandatory Service Instance configuration setting
                //whether or not to consider Daylight Savings Time when performing the calculations
                this.Service.ServiceConfiguration.Add("ConsiderDST", true, true);
            }
            catch (Exception ex)
            {
                // Record the exception message and indicate that this was an error.
                ServicePackage.ServiceMessages.Add(ex.Message, MessageSeverity.Error);
            }
            return base.GetConfigSection();
        }
        #endregion

        #region override string DescribeSchema()
        /// <summary>
        /// Describes the schema of the underlying data and services as equivalent SmartObject types
        /// This method is called whenever a service instance is registered or a service instance
        /// is refreshed
        /// </summary>
        /// <returns>A string containing the schema XML. The string is returned by executing the base.DescribeSchema() method
        /// after adding ServiceObjects to the this.Service.ServiceObjects collection</returns>
        public override string DescribeSchema()
        {
            try
            {
                //For this broker, we will add a single Service Object based on the definition of the TimeZoneCustom class
                //the TimeZoneCustomClass is decorated with attributed which allows the broker to create a Service Object by
                //interrogating the attributes of the class and properties/methods in the class

                //add the custom timezone class as a service object 
                this.Service.ServiceObjects.Create(new ServiceObject(typeof(TimeZoneCustomClass.TimeZoneCustom)));

                //set up the default values for the service instance
                this.Service.Name = "K2LearningStaticService";
                this.Service.MetaData.DisplayName = "K2Learning Custom Service Object (Static Schema)";
                this.Service.MetaData.Description = "This custom Service returns a statically defined TimeZone Service Object";

                // Indicate that the operation was successful.
                ServicePackage.IsSuccessful = true;

                //if you need to retrieve the configuration settings, do it this way:
                //string configValue = this.Service.ServiceConfiguration["ConfigItemName"].ToString();

                /*if you are using type mappings, set them up this way
                // This is a table which tells the service discovery method
                //how to map the native data types for the Provider to equivalent Service Object data types
                TypeMappings map = new TypeMappings();
                // Add type mappings.
                map.Add("Int32", SoType.Number);
                map.Add("String", SoType.Text);
                map.Add("Boolean", SoType.YesNo);
                map.Add("Date", SoType.DateTime);
                // Add the type mappings to the Service Instance.
                this.Service.ServiceConfiguration.Add("Type Mappings", map);
                */
            }
            catch (Exception ex)
            {
                // Record the exception message and indicate that this was an error.
                ServicePackage.ServiceMessages.Add(ex.Message, MessageSeverity.Error);
                // Indicate that the operation was unsuccessful.
                ServicePackage.IsSuccessful = false;
            }

            return base.DescribeSchema();
        }
        #endregion

        #region override void Extend()
        /// <summary>
        /// Extends the underlying system or technology's schema. This is only implemented for K2 SmartBox.
        /// </summary>
        public override void Extend()
        {
            try
            {
                throw new NotImplementedException("Service Object \"Extend()\" is not implemented.");
            }
            catch (Exception ex)
            {
                // Record the exception message and indicate that this was an error.
                ServicePackage.ServiceMessages.Add(ex.Message, MessageSeverity.Error);
                // Indicate that the operation was unsuccessful.
                ServicePackage.IsSuccessful = false;
            }
        }
        #endregion

        #endregion
    }
}
