using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace RandomNumberApp
{
    class Speech
    {
        private SpeechSynthesizer ts = new SpeechSynthesizer();
        public void AsyncTextSpeak(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            ts.SpeakCompleted += Ts_SpeakCompleted;
            ts.Rate = 1;
            if (ts.State == SynthesizerState.Ready)
            {
                PlayTextSpeak(text);
            }
        }

        private void Ts_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            Dispose();
        }

        private void PlayTextSpeak(string text)
        {
            ts.SetOutputToDefaultAudioDevice();
            ts.SelectVoice("Microsoft Heami Desktop");//한글 지원
                                                      //ts.SelectVoice("Microsoft David Desktop");//영문
                                                      //ts.SelectVoice("Microsoft Zira Desktop");//영문
                                                      //ts.SelectVoice("Microsoft mark Desktop");//영문
            ts.SpeakAsync(text);
        }

        public void PauseTextSpeak()
        {
            if (ts.State == SynthesizerState.Speaking)
            {
                ts.Pause();
            }
        }
        public void ResumeTextspeak()
        {
            if (ts.State == SynthesizerState.Paused)
            {
                ts.Resume();
            }
        }

        public void Dispose()
        {
            ts.Dispose();
        }
    }
}
