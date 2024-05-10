using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using BoilerPlate.App.Handlers.Options;
using BoilerPlate.Core.Extensions;
using BoilerPlate.Core.Utils;

namespace BoilerPlate.App.API.Extensions;

/// <summary> File storage settings </summary>
public static class FileStorageExtensions
{
    /// <summary> Add file storage </summary>
    public static void AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var options = services.AddServiceOptions<FileStorageOptions>(configuration);
        if (options.RootDirectory == null!)
        {
            throw new Exception("File storage root directory configuration is missing");
        }

        if (Directory.Exists(options.RootDirectory) || EnvUtils.IsSwaggerGen)
        {
            return;
        }

        var dirInfo = Directory.CreateDirectory(options.RootDirectory);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var dirSecurity = dirInfo.GetAccessControl();
            var administrators = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
            var currentUserName = WindowsIdentity.GetCurrent().Name;

            dirSecurity.AddAccessRule(
                new FileSystemAccessRule(
                    administrators,
                    FileSystemRights.FullControl,
                    InheritanceFlags.None,
                    PropagationFlags.NoPropagateInherit,
                    AccessControlType.Allow));

            dirSecurity.AddAccessRule(
                new FileSystemAccessRule(
                    currentUserName,
                    FileSystemRights.FullControl,
                    InheritanceFlags.None,
                    PropagationFlags.NoPropagateInherit,
                    AccessControlType.Allow));

            dirSecurity.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
        }
        else
        {
            var escapedRootDirPath = options.RootDirectory.Replace(" ", "\\ ");
            using var proc = Process.Start("/bin/bash", $"-c \"chmod 700 {escapedRootDirPath}\"");
            proc.WaitForExit();
            if (proc.ExitCode != 0)
            {
                throw new Exception("Error in changing access permissions to the file storage root directory");
            }
        }
    }
}