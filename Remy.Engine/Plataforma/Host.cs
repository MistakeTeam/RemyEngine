using Remy.Engine.Plataforma.Linux;
using Remy.Engine.Utility;

namespace Remy.Engine.Plataforma
{
    public static class Host
    {
        public static GameHost ChooseOSHost(string gameName)
        {
            switch (RuntimeInfo.OS)
            {
                case RuntimeInfo.Platform.Linux:
                    return new LinuxHost(gameName);
                default:
                    throw new InvalidOperationException($"Não foi possivél encontar um Host para seu sistema operacional: ({RuntimeInfo.OS}).");
            }
        }
    }
}