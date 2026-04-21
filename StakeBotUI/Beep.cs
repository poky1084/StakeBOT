using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakeBotUI
{
    public class AudioBeep
    {
        public async Task BeepAsync()
        {
            const double attack = 0.001;   // 1ms attack
            const double decay = 5.5;       // total duration
            const double tau = 0.30;        // decay time constant

            // Create WaveOut device
            var waveOut = new WaveOutEvent();

            // Create primary tone: 2642 Hz
            var osc1 = new SignalGenerator(44100, 1);
            osc1.Type = SignalGeneratorType.Sin;
            osc1.Frequency = 2642;
            osc1.Gain = 1.0;

            // Create bell overtone: 6869 Hz at 8% amplitude
            var osc2 = new SignalGenerator(44100, 1);
            osc2.Type = SignalGeneratorType.Sin;
            osc2.Frequency = 6869;
            osc2.Gain = 0.08;

            // Mix the two oscillators
            var mixer = new MixingSampleProvider(new[] { osc1, osc2 });
            mixer.ReadFully = true;

            // Apply envelope (attack/decay)
            var envelopeProvider = new EnvelopeSampleProvider(mixer, attack, decay, tau);

            // Initialize and play
            waveOut.Init(envelopeProvider);
            waveOut.Play();

            // Wait asynchronously for duration (doesn't block UI)
            await Task.Delay((int)(decay * 1000));

            // Cleanup
            waveOut.Stop();
            waveOut.Dispose();
        }

        // Custom SampleProvider for envelope shaping
        public class EnvelopeSampleProvider : ISampleProvider
        {
            private ISampleProvider source;
            private double attack;
            private double decay;
            private double tau;
            private double currentTime;
            private double envelopeValue;

            public WaveFormat WaveFormat => source.WaveFormat;

            public EnvelopeSampleProvider(ISampleProvider source, double attack, double decay, double tau)
            {
                this.source = source;
                this.attack = attack;
                this.decay = decay;
                this.tau = tau;
                this.currentTime = 0;
                this.envelopeValue = 0;
            }

            public int Read(float[] buffer, int offset, int count)
            {
                int samplesRead = source.Read(buffer, offset, count);
                int samplesPerSecond = WaveFormat.SampleRate;
                double deltaTime = 1.0 / samplesPerSecond;

                for (int i = 0; i < samplesRead; i++)
                {
                    // Attack phase (linear ramp up)
                    if (currentTime < attack)
                    {
                        envelopeValue = currentTime / attack;
                    }
                    // Decay phase (exponential decay)
                    else if (currentTime < decay)
                    {
                        double t = currentTime - attack;
                        envelopeValue = Math.Exp(-t / tau);
                    }
                    else
                    {
                        envelopeValue = 0;
                    }

                    // Apply envelope to sample
                    buffer[offset + i] *= (float)envelopeValue;
                    currentTime += deltaTime;

                    // Reset if we exceed decay time
                    if (currentTime >= decay)
                    {
                        envelopeValue = 0;
                    }
                }

                return samplesRead;
            }
        }
    }
}
