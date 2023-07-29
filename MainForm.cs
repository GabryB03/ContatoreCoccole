using MetroSuite;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading;

public partial class MainForm : MetroForm
{
    private int _currentSeconds;
    private ProtoRandom.ProtoRandom _random;
    private int _randomSeconds;

    public MainForm()
    {
        InitializeComponent();
        _random = new ProtoRandom.ProtoRandom(5);
        new Thread(UpdateTimer).Start();
        CheckForIllegalCrossThreadCalls = false;
        new Thread(SetRandomSeconds).Start();
        timer1.Start();
    }

    public void PlayStartSound()
    {
        PlaySound("sounds\\start.mp3");
    }

    public void PlayEndSound()
    {
        PlaySound("sounds\\end.mp3");
    }

    public void PlaySound(string fileName)
    {
        Mp3FileReader reader = new Mp3FileReader(fileName);
        WaveOutEvent waveOut = new WaveOutEvent();
        waveOut.Init(reader);
        waveOut.Play();
    }

    public void SetRandomSeconds()
    {
        _randomSeconds = _random.GetRandomInt32(int.Parse(File.ReadAllText("configs\\min_seconds_limit.txt")), int.Parse(File.ReadAllText("configs\\max_seconds_limit.txt")));

        while (true)
        {
            Thread.Sleep(1000);

            if (_randomSeconds == 0)
            {
                break;
            }

            _randomSeconds--;
        }

        PlayStartSound();
        new Thread(StartTimer).Start();
    }

    public void StartTimer()
    {
        _currentSeconds = int.Parse(File.ReadAllText("configs\\seconds_config.txt"));

        while (true)
        {
            Thread.Sleep(1000);

            if (_currentSeconds == 0)
            {
                break;
            }

            _currentSeconds--;
        }

        PlayEndSound();
        new Thread(SetRandomSeconds).Start();
    }

    public void UpdateTimer()
    {
        while (true)
        {
            int nCurrentHours = 0, nCurrentMinutes = 0, nTheSeconds = _currentSeconds;

            while (nTheSeconds >= 3600)
            {
                nTheSeconds -= 3600;
                nCurrentHours++;
            }

            while (nTheSeconds >= 60)
            {
                nTheSeconds -= 60;
                nCurrentMinutes++;
            }

            string currentHours = nCurrentHours.ToString(), currentMinutes = nCurrentMinutes.ToString(), currentSeconds = nTheSeconds.ToString();

            if (currentHours.Length == 1)
            {
                currentHours = "0" + currentHours;
            }

            if (currentMinutes.Length == 1)
            {
                currentMinutes = "0" + currentMinutes;
            }

            if (currentSeconds.Length == 1)
            {
                currentSeconds = "0" + currentSeconds;
            }

            label1.Text = currentHours + ":" + currentMinutes + ":" + currentSeconds;
        }
    }

    private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
    {
        Environment.Exit(0);
    }
}