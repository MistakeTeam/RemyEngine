namespace Remy.Extra
{
    public static class Letra
    {
        public static string FormatarLinha(string texto = null)
        {
            string dp = "--------------------------------------------------";
            if (texto != null)
            {
                dp = dp[..((dp.Length / 2) - (texto.Length / 2))];
                return string.Format(dp + texto.ToUpper() + dp);
            }
            else
            {
                return dp;
            }
        }
    }
}