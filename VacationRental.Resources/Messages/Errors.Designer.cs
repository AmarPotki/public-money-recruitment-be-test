﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VacationRental.Resources.Messages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VacationRental.Resources.Messages.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occur.Try it again..
        /// </summary>
        public static string ApplicationException {
            get {
                return ResourceManager.GetString("ApplicationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Booking not found..
        /// </summary>
        public static string BookingNotFound {
            get {
                return ResourceManager.GetString("BookingNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can not update PreparationTimeInDays because overlapping will happen..
        /// </summary>
        public static string NewPreparationTimeInDaysFails {
            get {
                return ResourceManager.GetString("NewPreparationTimeInDaysFails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can not update PreparationTimeInDays Or Unit count because overlapping will happen..
        /// </summary>
        public static string NewPreparationTimeInDaysOrUnitsFails {
            get {
                return ResourceManager.GetString("NewPreparationTimeInDaysOrUnitsFails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can not update Units because there is at least one day in your bookings list that .....
        /// </summary>
        public static string NewUnitsCountFails {
            get {
                return ResourceManager.GetString("NewUnitsCountFails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nights must be positive..
        /// </summary>
        public static string NightsMustBePositive {
            get {
                return ResourceManager.GetString("NightsMustBePositive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No change exist..
        /// </summary>
        public static string NoChange {
            get {
                return ResourceManager.GetString("NoChange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not available.
        /// </summary>
        public static string NotAvailable {
            get {
                return ResourceManager.GetString("NotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rental not found..
        /// </summary>
        public static string RentalNotFound {
            get {
                return ResourceManager.GetString("RentalNotFound", resourceCulture);
            }
        }
    }
}
