using MartinGC94.MonitorConfig.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Security;

namespace MartinGC94.MonitorConfig
{
    internal static class Utils
    {
        internal static WildcardPattern[] GetWildcardPatterns(IList<string> patterns, WildcardOptions options = WildcardOptions.IgnoreCase)
        {
            if (patterns is null || patterns.Count == 0)
            {
                return null;
            }

            var result = new WildcardPattern[patterns.Count];
            for (int i = 0; i < patterns.Count; i++)
            {
                result[i] = new WildcardPattern(patterns[i], options);
            }

            return result;
        }

        /// <summary>Gets a relevant error category based on the exception input.</summary>
        internal static ErrorCategory GetErrorCategory(this Exception exception)
        {
            switch (exception)
            {
                case Win32Exception nativeError:
                    switch (nativeError.NativeErrorCode)
                    {
                        case 2:
                            return ErrorCategory.ObjectNotFound;

                        case 5:
                        case 1314:
                            return ErrorCategory.PermissionDenied;

                        case 6:
                        case ERROR_NOT_SUPPORTED:
                            return ErrorCategory.InvalidOperation;

                        case 87:
                            return ErrorCategory.InvalidArgument;

                        case ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED:
                            return ErrorCategory.NotImplemented;

                        case ERROR_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE:
                        case ERROR_GRAPHICS_MONITOR_NO_LONGER_EXISTS:
                            return ErrorCategory.ResourceUnavailable;

                        default:
                            return ErrorCategory.NotSpecified;
                    }

                case ApiException apiError:
                    return GetErrorCategory(apiError.InnerException);

                case ArgumentNullException _:
                case ArgumentException _:
                    return ErrorCategory.InvalidArgument;

                case IOException _:
                case ObjectDisposedException _:
                    return ErrorCategory.ResourceUnavailable;

                case InvalidOperationException _:
                    return ErrorCategory.InvalidOperation;

                case UnauthorizedAccessException _:
                case SecurityException _:
                    return ErrorCategory.PermissionDenied;

                case InvalidCastException _:
                    return ErrorCategory.InvalidType;

                default:
                    return ErrorCategory.NotSpecified;
            }
        }

        private const int ERROR_NOT_SUPPORTED = 50;
        private const int ERROR_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE = -1071241844;
        private const int ERROR_GRAPHICS_MONITOR_NO_LONGER_EXISTS = -1071241843;
        private const int ERROR_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED = -1071241852;

        /// <summary>
        /// Returns true if the error indicates that no other operations can be performed, eg a handle is closed.
        /// </summary>
        internal static bool IsTerminatingError(this Win32Exception exception)
        {
            return IsTerminatingErrorCode(exception.NativeErrorCode);
        }

        internal static bool IsTerminatingErrorCode(int nativeErrorCode)
        {
            return nativeErrorCode == ERROR_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE ||
                nativeErrorCode == ERROR_GRAPHICS_MONITOR_NO_LONGER_EXISTS ||
                nativeErrorCode == ERROR_NOT_SUPPORTED;
        }
    }
}