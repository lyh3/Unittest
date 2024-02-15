using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Intel.MsoAuto.C3.Loader.EDW.Business.Core {
    /// <summary>
    /// Usage: 
    ///      using (WindowsInpersonateLogin wi = new WindowsInpersonateLogin(<user name>, <domain>, <pass word>))
    ///      {
    ///          WindowsIdentity.RunImpersonated(wi.Identity.AccessToken, () =>
    ///          {
    ///              WindowsIdentity useri = WindowsIdentity.GetCurrent();
    ///              Console.WriteLine(useri.Name);
    ///              your action();
    ///          });
    ///      }
    /// </summary>
    public class WindowsInpersonateLogin : IDisposable {
        protected const int LOGON32_PROVIDER_DEFAULT = 0;
        protected const int LOGON32_LOGON_INTERACTIVE = 2;

        public WindowsIdentity? Identity = null;
        private IntPtr accessToken;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain,
        string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private extern static bool CloseHandle(IntPtr handle);
        public WindowsInpersonateLogin()
        {
            Identity = WindowsIdentity.GetCurrent();
        }
        public WindowsInpersonateLogin(string username, string domain, string password)
        {
            Login(username, domain, password);
        }
        public void Login(string username, string domain, string password)
        {
            if (Identity != null)
            {
                Identity.Dispose();
                Identity = null;
            }
            try
            {
                accessToken = new IntPtr(0);
                Logout();

                accessToken = IntPtr.Zero;
                bool logonSuccessfull = LogonUser(
                   username,
                   domain,
                   password,
                   LOGON32_LOGON_INTERACTIVE,
                   LOGON32_PROVIDER_DEFAULT,
                   ref accessToken);

                if (!logonSuccessfull)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(error);
                }
                Identity = new WindowsIdentity(accessToken);
            }
            catch
            {
                throw;
            }
        }
        public void Logout()
        {
            if (accessToken != IntPtr.Zero)
                CloseHandle(accessToken);
            accessToken = IntPtr.Zero;
            if (Identity != null)
            {
                Identity.Dispose();
                Identity = null;
            }
        }
        void IDisposable.Dispose()
        {
            Logout();
        }
    }
}