﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMR.Patient.Resources {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ErrorMessages {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessages() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("MMR.Patient.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string CannotCreatePatientProfileTitle {
            get {
                return ResourceManager.GetString("CannotCreatePatientProfileTitle", resourceCulture);
            }
        }
        
        internal static string CannotUpdatePatientProfileTitle {
            get {
                return ResourceManager.GetString("CannotUpdatePatientProfileTitle", resourceCulture);
            }
        }
        
        internal static string PatientProfileExistsErrorDetails {
            get {
                return ResourceManager.GetString("PatientProfileExistsErrorDetails", resourceCulture);
            }
        }
        
        internal static string PatientProfileDoesNotExistErrorDetails {
            get {
                return ResourceManager.GetString("PatientProfileDoesNotExistErrorDetails", resourceCulture);
            }
        }
        
        internal static string MustBePastDate {
            get {
                return ResourceManager.GetString("MustBePastDate", resourceCulture);
            }
        }
        
        internal static string MinBirthdayMessage {
            get {
                return ResourceManager.GetString("MinBirthdayMessage", resourceCulture);
            }
        }
        
        internal static string MustBeMaleOrFemale {
            get {
                return ResourceManager.GetString("MustBeMaleOrFemale", resourceCulture);
            }
        }
    }
}
