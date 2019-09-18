using UnityEngine.Networking;

namespace Assets.Requests
{
    public class CustomCertificateHandler: CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}
