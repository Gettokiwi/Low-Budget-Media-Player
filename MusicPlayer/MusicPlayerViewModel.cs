using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;


namespace MusicPlayer 
{
    public class MusicPlayerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Private Fields
        /// </summary>
        private Media _currentMedia;
        private Media _newMedia;
        private int _index;
        private int _shuffledIndex;
        private int _helperIndex;
        private double _volume;
        private double _durationInDouble;
        private double _mediaLenght;
        private string _displayName;
        private string _duration;
        private bool _isShuffleOn;
        private bool _isVideoPlaying;
        private MediaPlayer MediaPlayer = new MediaPlayer();
        private DispatcherTimer dispatcherTimer;
        public DelegateCommand nextCommand { get; private set; }
        public DelegateCommand previousCommand { get; private set; }
        public DelegateCommand playCommand { get; private set; }
        public DelegateCommand openCommand { get; private set; }
        public DelegateCommand pauseCommand { get; private set; }
        public DelegateCommand stopCommand { get; private set; }
        public ObservableCollection<Media> Medias { get; private set; }
        public ObservableCollection<Media> ShuffledMedia { get; private set; }
        /// <summary>
        /// Public Fields
        /// </summary>
        public Media CurrentMedia
        {
            get { return _currentMedia; }
            set
            {
                if (_currentMedia != value)
                {


                    _currentMedia = value;
                    if (Path.GetExtension(CurrentMedia.PathString) == ".avi" || Path.GetExtension(CurrentMedia.PathString) == ".mp4")
                        IsVideoPlaying = true;

                    if (Medias.Count > 0)
                    {
                        if (IsShuffleOn)
                        {

                            MediaPlayer.Open(new Uri(ShuffledMedia[ShuffledIndex].PathString));

                        }

                        else
                        {

                            MediaPlayer.Open(new Uri(Medias[Index].PathString));
                        }


                    }


                    DoPlay();
                    OnPropertyChanged("CurrentMedia");
                }
            }

        }
        public Media NewMedia
        {
            get { return _newMedia; }
            set
            {
                if (_newMedia != value)
                {
                    _newMedia = value;
                    OnPropertyChanged("NewMedia");
                }
            }

        }
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    
                    
                    
                        if (value < 0)
                            value = Medias.Count - 1;

                        if (value > Medias.Count - 1)
                            value = 0;

                        _index = value;
                    
                        CurrentMedia = Medias[_index];
                        if(IsShuffleOn)
                            ShuffledIndex = ShuffledMedia.IndexOf(CurrentMedia);
                        OnPropertyChanged("Index");

                    
                    
                }
            }
        }
        public int ShuffledIndex
        {
            get { return _shuffledIndex; }
            set
            {
                if(_shuffledIndex != value)
                {

                    if (value < 0)
                        value = ShuffledMedia.Count - 1;

                    if (value > ShuffledMedia.Count - 1)
                        value = 0;
                    _shuffledIndex = value;
                    CurrentMedia = ShuffledMedia[value];
                    HelperIndex = Medias.IndexOf(CurrentMedia);
                    OnPropertyChanged("ShuffledIndex");
                }
            }
                
        }
        public int HelperIndex
        {
            get { return _helperIndex; }
            set
            {
                if (_helperIndex != value)
                {
                    _helperIndex = value;
                    OnPropertyChanged("HelperIndex");
                }
            }

        }
        public double Volume
        {
            get { return _volume; }
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    MediaPlayer.Volume = value;
                    OnPropertyChanged("Volume");
                }
            }

        }
        public double DurationInDouble
        {
            get { return _durationInDouble; }
            set
            {
                if (_durationInDouble != value)
                {
                    _durationInDouble = value;
  
                    MediaPlayer.Position = TimeSpan.FromSeconds(value);
                    OnPropertyChanged("DurationInDouble");
                }
            }

        }
        public double MediaLenght
        {
            get { return _mediaLenght; }
            set
            {
                if (_mediaLenght != value)
                {
                    _mediaLenght = value;
                    OnPropertyChanged("MediaLenght");
                }
            }

        }
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }

        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    
                    OnPropertyChanged("Duration");
                }
            }

        }
        public bool IsShuffleOn
        {
            get { return _isShuffleOn; }
            set
            {
                if (_isShuffleOn != value)
                {
                    _isShuffleOn = value;
                    if (IsShuffleOn)
                    {
                        Shuffle();
                    }
                    OnPropertyChanged("IsShuffleOn");
                }
            }

        }
        public bool IsVideoPlaying
        {
            get { return _isVideoPlaying; }
            set
            {
                if (_isVideoPlaying != value)
                {
                    _isVideoPlaying = value;
                    
                    OnPropertyChanged("IsVideoPlaying");
                }
            }
        }
       

        public MusicPlayerViewModel()
        {
            Medias = new ObservableCollection<Media>();
            ShuffledMedia = new ObservableCollection<Media>();
            CurrentMedia = new Media();
            NewMedia = new Media();
            MediaPlayer.Volume = 0.5;
            Volume = 0.5;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            nextCommand = new DelegateCommand(param => DoNext());
            previousCommand = new DelegateCommand(param => DoPrevious());
            playCommand = new DelegateCommand(param => DoPlay());
            openCommand = new DelegateCommand(param => DoOpen());
            pauseCommand = new DelegateCommand(param => DoPause());
            stopCommand = new DelegateCommand(param => DoStop());
        }
        /// <summary>
        /// Commands
        /// </summary>

        public void DoNext()
        {
            if (IsShuffleOn)
            {
                ShuffledIndex = ShuffledMedia.IndexOf(CurrentMedia) + 1;
                //Index = Medias.IndexOf(CurrentMedia);
                
            }
            else
            {
                Index = Medias.IndexOf(CurrentMedia) + 1;
                
            }
            
            


        }
        public void DoPrevious()
        {
            if(IsShuffleOn)
            {
                ShuffledIndex = ShuffledMedia.IndexOf(CurrentMedia) - 1;
 
            }
            else
            {
                Index = Medias.IndexOf(CurrentMedia) - 1;
       
            }
            
            
            


        }
        public void DoPlay()
        {
            
            MediaPlayer.Play();
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                dispatcherTimer = new DispatcherTimer();
                MediaLenght =Math.Round(MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds);
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            }
            

        }
        public void DoStop()
        {
            MediaPlayer.Stop();
        }
        public void DoOpen()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Avi files (*.avi)|*.avi|Mp3 files (*.mp3)|*.mp3|Mp4 files (*.mp4)|*.mp4|All files (*.*)|*.*";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                NewMedia = new Media();
                string path = dialog.FileName;
                NewMedia.PathString = path;
                DisplayName = Path.GetFileName(path);
                NewMedia.Name = DisplayName;

                if (Medias.Count == 0)
                {
                    CurrentMedia = NewMedia;
                    MediaPlayer.Open(new Uri(NewMedia.PathString));


                }
                
                Medias.Add(NewMedia);


            }
            
            OnPropertyChanged("DoOpen");

        }
        public void DoPause()
        {
            MediaPlayer.Pause();
        }

        private void Shuffle()
        {


            Media rndMusic;
            ShuffledIndex = 0;
            ShuffledMedia.Clear();
            foreach (Media music in Medias)
            {
                ShuffledMedia.Add(music);
            }
            for (int i = 0; i < ShuffledMedia.Count-1; i++)
            {
                Random rnd = new Random();
                rndMusic = new Media();
                int temp = 0;
                temp = rnd.Next(ShuffledMedia.Count);
                rndMusic = ShuffledMedia[temp];
                ShuffledMedia[temp] = ShuffledMedia[i];
                ShuffledMedia[i] = rndMusic;

            }
            if(ShuffledMedia[0] != CurrentMedia)
            {
                rndMusic = CurrentMedia;
                ShuffledMedia.Remove(CurrentMedia);
                ShuffledMedia.Insert(0, rndMusic);
                
            }

            
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                if(MediaPlayer.NaturalDuration.TimeSpan.TotalMinutes <= 60)
                    Duration = String.Format("{0} / {1}", MediaPlayer.Position.ToString(@"mm\:ss"), MediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                else
                    Duration = String.Format("{0} / {1}", MediaPlayer.Position.ToString(@"hh\:mm\:ss"), MediaPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));

                DurationInDouble = MediaPlayer.Position.TotalSeconds;
                if (MediaPlayer.Position == MediaPlayer.NaturalDuration.TimeSpan)
                {
                    if (Medias.Count > 1)
                    {
                        DoNext();
                    }
                    else
                    {
                        CurrentMedia = Medias[0];
                    }
                }
            }
           
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
