namespace Remy.Extra
{
    ///<Summary>
    /// Classe calculadora, concentrando todas as funções matemáticas.
    ///</Summary>
    public static class Math
    {
        ///<Summary>
        /// Pegar a porcentagem de um valor.
        ///</Summary>
        public static float PegarPorcentagem(float valor, float total)
        {
            return (valor / total) * 100;
        }

        public static float PegarValorPorcentagem(float porcento, float total)
        {
            if(porcento>100) porcento = 100;
            return (total / porcento) * 100;
        }
    }
}
