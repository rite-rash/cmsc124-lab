using System;
using System.Collections.Generic;
using System.Diagnostics; //for Process and ProcessStartInfo
using System.IO; //for finding the folder
using System.Text; //conver RIFF, WAVEfmt, data to ASCII for .wav file header


namespace Ck
{
    public class PlayNote : ICallable
    {
        public int ParamsCount() => 2; // playNote(midi, seconds)
        public object? Call(Evaluator evaluator, List<object?> args)
        {
            double midi = (double)args[0]; // num of key in piano
            double duration = (double)args[1];

            //get frequency using midi
            double frequency = 440.0 * Math.Pow(2.0, (midi - 69.0) / 12.0);

            //path of file to send to OS
            //.Combine and .GetTempPath depends on which OS 
            string pathFile = Path.Combine(Path.GetTempPath(), "ck_note.wav");

            //header for .wav
            WriteWav(pathFile, frequency, duration);
            //file to give to OS
            PlayFile(pathFile);

            return null;
        }

        private static void PlayFile(string filePath)
        {
            //object that holds instruc for launching an external  program
            ProcessStartInfo startInfo = new ProcessStartInfo();

        }
        private static void WriteWav(string filePath, double frequency, double duration)
        {
            // TODO: write 44-byte RIFF/WAVE header and PCM samples
        }

    }




}