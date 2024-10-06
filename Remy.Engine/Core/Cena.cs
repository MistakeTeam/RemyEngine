using System.Security.Cryptography;

namespace Remy.Engine.Core
{
    public class Cena
    {
        private short _id;
        public short ID
        {
            get => _id = (short)GCena.Cenas.Count;
            set
            {
                if (GCena.Cenas.Where(r => r.ID == (short)GCena.Cenas.Count) != null)
                    _id = (short)(GCena.Cenas.Count + 1);
                else
                    _id = (short)GCena.Cenas.Count;
            }
        }

        public string Nome;

        public Cena(string nome)
        {
            Nome = nome;
        }
    }
}