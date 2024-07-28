namespace Remy.Cliente.Utility
{
    public class Converter
    {
        public static float Intervalo(float x, float oMin, float oMax, float nMin, float nMax)
        {
            if (oMin == oMax)
            {
                Console.WriteLine("Zero input range");
                return float.NaN;
            }

            if (nMin == nMax)
            {
                Console.WriteLine("Zero output range");
                return float.NaN;
            }

            bool reverseInput = false;
            float oldMin = Math.Min(oMin, oMax);
            float oldMax = Math.Max(oMin, oMax);

            if (oldMin != oldMax)
                reverseInput = true;

            bool reverseOutput = false;
            float newMin = Math.Min(nMin, nMax);
            float newMax = Math.Max(nMin, nMax);

            if (newMin != newMax)
                reverseOutput = true;

            float portion = (x - oldMin) * (newMax - newMin) / (oldMax - oldMin);
            if (reverseInput)
                portion = (oldMax - x) * (newMax - newMin) / (oldMax - oldMin);

            float resultado = portion + newMin;
            if (reverseOutput)
                resultado = newMax - portion;

            return resultado;
        }
    }
}