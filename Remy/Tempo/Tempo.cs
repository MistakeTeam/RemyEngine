using System.Diagnostics;

namespace Remy.Tempo
{
    public class Tempo
    {
        // Deus do tempo. Tudo começa por ele...!
        private Stopwatch Cronos = new Stopwatch();
        private readonly long[] _realFrameTimes = new long[NumQuadros];
        private int _frameIdx;
        private const int NumQuadros = 60;
        public int QuadroPorSegundo = NumQuadros;
        public TimeSpan UltimoTempo;
        public TimeSpan TempoReal => Cronos.Elapsed;
        public TimeSpan TempoQuadro { get; private set; }
        public TimeSpan RealFrameTimeStdDev => CalcRftStdDev();
        public double FPS => CalcMediaFPS();
        public TimeSpan LastTick { get; set; }
        public byte TickRate;
        public TimeSpan TickPeriod => TimeSpan.FromTicks((long)(1.0 / TickRate * TimeSpan.TicksPerSecond));
        public uint CurFrame { get; set; } = 1;
        public float TickTimingAdjustment { get; set; } = 0;
        public bool Simulação { get; set; }
        public bool Pausado { get; set; }
        public TimeSpan TempoAtual;
        public TimeSpan MediaQuadros => TimeSpan.FromTicks((long)_realFrameTimes.Average());

        public Tempo()
        {
            Cronos.Start();

            Pausado = true;
            TickRate = NumQuadros;
        }

        public TimeSpan CalcAdjustedTickPeriod()
        {
            var ratio = Math.Clamp(TickTimingAdjustment, -0.99f, 0.99f);

            return TickPeriod * (1 - ratio);
        }

        public void IniciarQuadro()
        {
            UltimoTempo = TempoAtual;
            TempoAtual = TempoReal;
            TempoQuadro = TempoAtual - UltimoTempo;

            _frameIdx = (1 + _frameIdx) % _realFrameTimes.Length;
            _realFrameTimes[_frameIdx] = TempoQuadro.Ticks;
        }

        private double CalcMediaFPS()
        {
            return 1 / (_realFrameTimes.Average() / TimeSpan.TicksPerSecond);
        }

        private TimeSpan CalcRftStdDev()
        {
            var sum = _realFrameTimes.Sum();
            var count = _realFrameTimes.Length;
            var avg = sum / (double)count;
            double devSquared = 0.0f;
            for (var i = 0; i < count; ++i)
            {
                if (_realFrameTimes[i] == 0)
                    continue;

                var ft = _realFrameTimes[i];

                var dt = ft - avg;

                devSquared += dt * dt;
            }

            var variance = devSquared / (count - 1);
            return TimeSpan.FromTicks((long)Math.Sqrt(variance));
        }
    }
}