namespace Remy.Engine.Utility
{
    public class FuncMath
    {
        /// <summary>
        /// Tone mapping é uma técnica que ajusta a aparência de imagens de alta faixa dinâmica (HDR) para que sejam exibidas em dispositivos com faixa dinâmica mais limitada (SDR).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="E"></param>
        /// <param name="F"></param>
        /// <returns></returns>
        double Uncharted2ToneMapping(double x, double A, double B, double C, double D, double E, double F)
        {
            return ((x * (A * x + C * B) + D * E) / (x * (A * x + B) + D * F)) - E / F;
        }
    }
}