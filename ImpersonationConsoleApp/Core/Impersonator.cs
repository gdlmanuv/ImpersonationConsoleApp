using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ImpersonationConsoleApp.Core
{
    public partial class Impersonator : IDisposable
    {
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        private WindowsImpersonationContext _impersonationContext;

        public Impersonator(string userName, string domainName, string password)
        {
            ImpersonateValidUser(userName, domainName, password);
        }

        public void Dispose()
        {
            UndoImpersonation();
        }

        private void ImpersonateValidUser(string userName, string domain, string password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            try
            {
                if (RevertToSelf())
                {
                    if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            _impersonationContext = tempWindowsIdentity.Impersonate();
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }

                if(tokenDuplicate != IntPtr.Zero)
                {
                    CloseHandle(tokenDuplicate);
                }
            }
        }

        private void UndoImpersonation()
        {
            if (_impersonationContext != default)
            {
                _impersonationContext.Undo();
            }
        }
    }
}
