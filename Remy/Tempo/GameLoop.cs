using Remy.Logs;
using Remy.Render;

namespace Remy.Tempo;

public class GameLoop
{
    public bool Running { get; set; }

    private readonly Tempo Loop = new Tempo();
    private TimeSpan _lastKeepUp;
    public int MaxQueuedTicks { get; set; } = 5;

    public GameLoop() { }

    public void Iniciar()
    {
        Running = true;
        while (Running)
        {
            Console.Clear();
            Console.WriteLine("----------TEMPO----------");

            var maxTime = TimeSpan.FromTicks(Loop.TickPeriod.Ticks * MaxQueuedTicks);

            var accumulator = Loop.TempoAtual - Loop.LastTick;

            Console.WriteLine("[accumulator & maxTime]: {0} / {1}", accumulator, maxTime);

            if (accumulator > maxTime)
            {
                accumulator = maxTime;

                Loop.LastTick = Loop.TempoAtual - maxTime;

                if ((Loop.TempoAtual - _lastKeepUp).TotalSeconds >= 15.0)
                {
                    _lastKeepUp = Loop.TempoAtual;
                }
            }

            Loop.IniciarQuadro();

            //////////////////////////////////////////////////////////////////////////
            /// ATUALIZAR SIMULAÇÃO
            Loop.Simulação = true;

            var tickPeriod = Loop.CalcAdjustedTickPeriod();

            while (accumulator >= tickPeriod)
            {
                accumulator -= tickPeriod;
                Loop.LastTick += tickPeriod;
                Console.WriteLine("[accumulator & LastTick]: {0} / {1}", accumulator, Loop.LastTick);

                if (Loop.Pausado)
                    continue;
            }

            Loop.Simulação = false;
            /// FIM DA ATUALIZAÇÃO DA SIMULAÇÃO
            //////////////////////////////////////////////////////////////////////////

            Console.WriteLine("[Tempo atual]: {0}", Loop.TempoAtual);
            Console.WriteLine("[Diferença de tempo]: {0}", Loop.TempoQuadro);
            Console.WriteLine("[Ultimo tempo]: {0}", Loop.UltimoTempo);
            Console.WriteLine("[tickPeriod]: {0}", tickPeriod);
            Console.WriteLine("[FPS]: {0}", Loop.FPS);
            Console.WriteLine("[RealFrameTimeStdDev]: {0}", Loop.RealFrameTimeStdDev);
            Console.WriteLine("[Media de quadros]: {0}", Loop.MediaQuadros);

            Thread.Sleep(TimeSpan.FromMilliseconds(1000 / Loop.QuadroPorSegundo));
        }
    }
}