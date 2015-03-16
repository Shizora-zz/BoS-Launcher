using Awesomium.Core;
using System;
using System.Windows;

namespace BoS_Launcher.Tools.Helper
{
    public class ResourceInterceptor : IResourceInterceptor
    {

        public virtual bool OnFilterNavigation(NavigationRequest request)
        {
            if (request.Url.Scheme.Equals("ts3server", StringComparison.InvariantCultureIgnoreCase))
            {
                // explicitly prefix the run command with ts3server:, just in case a malicious user subverts the System.Uri parser
                try
                {
                    System.Diagnostics.Process.Start(request.Url.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
              
                return true;
            }

            return false;
        }

        public ResourceResponse OnRequest(ResourceRequest request)
        {
            return ResourceResponse.Create(request.Url.OriginalString);
        }
    }
}
