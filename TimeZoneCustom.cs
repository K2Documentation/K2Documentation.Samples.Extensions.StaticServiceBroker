using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using Attributes = SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace TimeZoneCustomClass
{
    // The class is decorated with a ServiceObject Attribute 
    // which will make it available as a Service Object
    [Attributes.ServiceObject("CustomTimeZone", "Custom Time Zone", "Customized implementation of Time Zone data")]
    public class TimeZoneCustom
    {
        #region Private Members
        private DateTime baseTime; //the BaseTime sets the time to which all calculations are performed. If this value is not initialized, we will use the current (System) date and time
        private string name; //name of time zone depending on whether it is currently in DST or not
        private string fullName;	//a long display name for the time zone
        private string standardName; //standard name for this time zone
        private string daylightName; //name of Daylight Saving Time Zone
        private bool supportsDST; //whether the time zone support Daylight savings time
        private bool isDST; //whether the timezone is in DST at the specified time
        private Int32 utcOffSetMinutes; //number of minutes the timezone is offset to UTC
        private Int32 systemTimeOffSetMinutes; //number of minutes the local time differs from the system (machine) time
        private DateTime localTime; //the current time for the time zone
        #endregion

        /// <summary>
        /// This property is required if you want to get the service instance configuration 
        /// settings in this class
        /// </summary>
        private ServiceConfiguration _serviceConfig;
        public ServiceConfiguration ServiceConfiguration
        {
            get { return _serviceConfig; }
            set { _serviceConfig = value; }
        }

        /// <summary>
        /// Boolean indicator whether to cater for Daylight Savings time when calculating the local date time
        /// this value is set by a service instance configuration setting
        /// </summary>
        private bool _caterForDST
        {
            get { return System.Boolean.Parse(ServiceConfiguration["ConsiderDST"].ToString()); }
        }

        #region Public Properties (Exposed as Service Object Properties)
        // Each Public Property is decorated with a Property Attribute 
        // this makes the property available as a Service Object Property

        [Attributes.Property("BaseTime", SoType.DateTime, "BaseTime", "A base time to perform all other calculations from.")]
        public DateTime BaseTime
        {
            get
            {
                if (baseTime == null || baseTime.Year == 0001)
                {
                    return System.DateTime.Now;
                }
                else
                {
                    return baseTime;
                }
            }
            set { baseTime = value; }
        }

        [Attributes.Property("Name", SoType.Text, "Name", "A normalized name for the time zone, depending on whether it is in DST or not")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Attributes.Property("FullName", SoType.Text, "Full Name", "The full descriptive name of the time zone")]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        [Attributes.Property("StandardName", SoType.Text, "Standard Name", "The name of the Time Zone when in Standard Time")]
        public string StandardName
        {
            get { return standardName; }
            set { standardName = value; }
        }

        [Attributes.Property("DaylightName", SoType.Text, "DaylightName", "The name of the time zone when in DST")]
        public string DaylightName
        {
            get { return daylightName; }
            set { daylightName = value; }
        }

        [Attributes.Property("SupportsDST", SoType.YesNo, "Supports DST", "Whether the time zone supports Daylight Savings Time")]
        public bool SupportsDST
        {
            get { return supportsDST; }
            set { supportsDST = value; }
        }

        [Attributes.Property("IsDST", SoType.YesNo, "Is DST", "Whether the time zone is currently in Daylight Savings Time")]
        public bool IsDST
        {
            get { return isDST; }
            set { isDST = value; }
        }

        [Attributes.Property("UTCOffSetMinutes", SoType.Number, "UTC OffSet Minutes", "The number of minutes that the time differs from Coordinated Universal Time")]
        public Int32 UTCOffSetMinutes
        {
            get { return utcOffSetMinutes; }
            set { utcOffSetMinutes = value; }
        }

        [Attributes.Property("SystemTimeOffSetMinutes", SoType.Number, "System Time OffSet Minutes", "The number of minutes that the time differs from the current system time")]
        public Int32 SystemTimeOffSetMinutes
        {
            get { return systemTimeOffSetMinutes; }
            set { systemTimeOffSetMinutes = value; }
        }

        [Attributes.Property("LocalTime", SoType.DateTime, "Local Time", "The local time for the time zone")]
        public DateTime LocalTime
        {
            get { return localTime; }
            set { localTime = value; }
        }

        #endregion

        #region Service Object Methods

        //we create separate methods and decorate them with the Method Attribute. This is recommended so that you can interrogate the required properties, input properties and return properties to build up a return object
        
        //the first method is the LIst method that returns a collection of time zones
        [Attributes.Method("ListTimeZones", MethodType.List, "List TimeZones", "List all known timezones", new string[] { }, new string[] { "BaseTime" }, new string[] { "BaseTime", "Name", "FullName", "StandardName", "DaylightName", "SupportsDST", "IsDST", "UTCOffSetMinutes", "SystemTimeOffSetMinutes", "LocalTime" })]
        public TimeZoneCustom[] ListTimeZones()
        {
            TimeZoneCustom[] timezones = null;
            timezones = GetTimeZonesInfo(_caterForDST);
            return timezones;
        }

        //The next Service Object method is a Read method
        //This particular method has one optional input property (if BaseTime is set, calculate timezones using the basetime). 
        //there is a required input property for the timezone name (Required properties should always be included in the Input Properties collection)
        //and the method returns the full set of custom timezone Properties
        [Attributes.Method("ReadTimeZone", MethodType.Read, "Read TimeZone", "Read specific timezone", 
            new string[] { "Name" }, 
            new string[] { "Name", "BaseTime" }, 
            new string[] { "BaseTime", "Name", "FullName", "StandardName", "DaylightName", "SupportsDST", "IsDST", "UTCOffSetMinutes", "SystemTimeOffSetMinutes", "LocalTime" })]
         public TimeZoneCustom ReadTimeZone()
        {
            TimeZoneCustom tz = null;
            //retrieve the value of the time zone input property by querying the same property of the class.
            //behind the scenes K2 will set the properties for the class based on the input parameters provided by the user. 
            string tzName = Name;

            //we'll check if the field is blank or empty, even through it is required and should therefore never be blank
            if (string.IsNullOrEmpty(tzName))
            {
                throw new Exception("Time Zone Name is required");
            }

            tz = GetTimeZoneInfo(tzName, _caterForDST);

            return tz;
        }

        #endregion

        //default constructor. No implementation is required, but you must have a default public constructor
        public TimeZoneCustom()
        {

        }

        #region Internal Methods

        //override constructor to accept a base datetime
        private TimeZoneCustom(DateTime baseDateTime, TimeZoneInfo tzi, bool CaterForDST)
        {
            DateTime currentTimeUTC = baseDateTime.ToUniversalTime();

            if (tzi.IsDaylightSavingTime(currentTimeUTC))
            {
                this.Name = tzi.DaylightName;
            }
            else
            {
                this.Name = tzi.StandardName;
            }
            this.BaseTime = baseDateTime;
            this.FullName = tzi.DisplayName;
            this.StandardName = tzi.StandardName;
            this.DaylightName = tzi.DaylightName;
            this.SupportsDST = tzi.SupportsDaylightSavingTime;
            this.IsDST = tzi.IsDaylightSavingTime(currentTimeUTC);

            DateTime localDate = currentTimeUTC;
            TimeSpan timeDiffUtcClient = tzi.BaseUtcOffset;
            double totalMinutes = timeDiffUtcClient.TotalMinutes;
            localDate = baseDateTime.ToUniversalTime().Add(timeDiffUtcClient);
            //calc the current date for the timezone using DST, if we need to consider DST
            //courtesy of http://stackoverflow.com/questions/3219558/how-find-timezone-and-return-time-with-daylightsavingtime-applied
            if (CaterForDST && tzi.SupportsDaylightSavingTime && tzi.IsDaylightSavingTime(localDate))
            {
                TimeZoneInfo.AdjustmentRule[] rules = tzi.GetAdjustmentRules();
                foreach (var adjustmentRule in rules)
                {
                    if (adjustmentRule.DateStart <= localDate && adjustmentRule.DateEnd >= localDate)
                    {
                        localDate = localDate.Add(adjustmentRule.DaylightDelta);
                        timeDiffUtcClient = adjustmentRule.DaylightDelta;
                        totalMinutes = totalMinutes + timeDiffUtcClient.TotalMinutes;
                    }
                }
            }

            this.LocalTime = localDate;
            this.UTCOffSetMinutes = System.Convert.ToInt32(totalMinutes);
            this.SystemTimeOffSetMinutes = System.Convert.ToInt32((localDate - baseDateTime.ToLocalTime()).TotalMinutes);
        }

        //internal method to return a collection of Time Zones
        public TimeZoneCustom[] GetTimeZonesInfo(bool CaterForDST)
        {
            List<TimeZoneCustom> timeZones = new List<TimeZoneCustom>();

            DateTime currentTimeUTC = BaseTime.ToUniversalTime();
            
            //get a list of all the timezone definitions
            foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
            {
                TimeZoneCustom tz = new TimeZoneCustom(BaseTime, tzi, CaterForDST);
                timeZones.Add(tz);
            }
            return timeZones.ToArray();
        }

        //this method returns timezoneinfo for a specific date time. We assume that the datetime has already been converted to UTC
        public TimeZoneCustom[] GetTimeZonesInfoForSpecificUTCDate(DateTime targetDateTime, bool CaterForDST)
        {
            List<TimeZoneCustom> timeZones = new List<TimeZoneCustom>();
            this.BaseTime = targetDateTime;

            //get a list of all the timezone definitions
            foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
            {
                TimeZoneCustom tz = new TimeZoneCustom(BaseTime, tzi, CaterForDST);
                timeZones.Add(tz);
            }
            return timeZones.ToArray();
        }

        //this method returns timezoneinfo for a specific time zone name for the current UTC time.
        //if no equivalent time zone is found, null is returned.
        public TimeZoneCustom GetTimeZoneInfo(string TimeZoneName, bool CaterForDST)
        {
            //get a list of all the timezone definitions
            foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
            {
                if (string.Compare(TimeZoneName,tzi.StandardName,true) == 0 || (string.Compare(TimeZoneName,tzi.DaylightName,true) == 0))
                {
                    TimeZoneCustom tz = new TimeZoneCustom(BaseTime, tzi, CaterForDST);
                    return tz;
                }
            }
            return null;
        }

        //this method returns timezoneinfo for a specific time zone name for a specific UTC DateTime.
        //if no equivalent time zone is found, null is returned.
        public TimeZoneCustom GetTimeZoneInfoForSpecificUTCDate(string TimeZoneName, DateTime targetDateTime, bool CaterForDST)
        {
            this.BaseTime = targetDateTime;

            //get a list of all the timezone definitions
           foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
            {

                if (string.Compare(TimeZoneName,tzi.StandardName,true) == 0 || (string.Compare(TimeZoneName,tzi.DaylightName,true) == 0))
                {
                    TimeZoneCustom tz = new TimeZoneCustom(BaseTime, tzi, CaterForDST);
                    return tz;
                }
            }
            return null;
        }

        #endregion
    }
}
