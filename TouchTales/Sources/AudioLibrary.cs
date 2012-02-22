/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Syderis.CellSDK.Core.Sounds;
using Syderis.CellSDK.Core;
#endregion

namespace TouchyTales
{
    public class AudioLibrary
    {
        #region Constants and Statics
        /// <summary>
        /// Constants
        /// </summary>
        private const string path = "Sounds/";
        public const int SBALL = 0;
        public const int SROPE = 1;
        public const int SDOLL1 = 2;
        public const int SDOLL2 = 3;
        public const int SPIPIN = 4;
        public const int STRAIN = 5;
        public const int SBALL2 = 6;
        private const int NUM_SOUNDS = 7;
        /// <summary>
        /// Statics
        /// </summary>
        private static AudioLibrary instance;
        #endregion

        #region Variables
        private SoundInstance[] sounds; 
        #endregion

        #region Properties
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static AudioLibrary Instance
        {
            get
            {
                if (instance == null)
                    instance = new AudioLibrary();
                return instance;
            }
        } 
        #endregion

        #region Constructor
        /// <summary>
        /// Private constructor (Singleton)
        /// </summary>
        private AudioLibrary()
        {
            sounds = new SoundInstance[NUM_SOUNDS];
        } 
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize all sound components
        /// </summary>
        public void Initialize()
        {
            sounds[SBALL] = StaticContent.Resources.CreateSound(path + "Treeball").CreateInstance();
            sounds[SROPE] = StaticContent.Resources.CreateSound(path + "RopeShort").CreateInstance();
            sounds[SDOLL1] = StaticContent.Resources.CreateSound(path + "DollLove").CreateInstance();
            sounds[SDOLL2] = StaticContent.Resources.CreateSound(path + "DollLaught").CreateInstance();
            sounds[SPIPIN] = StaticContent.Resources.CreateSound(path + "Bird").CreateInstance();
            sounds[STRAIN] = StaticContent.Resources.CreateSound(path + "ToyTrain").CreateInstance();
            sounds[SBALL2] = StaticContent.Resources.CreateSound(path + "Ball").CreateInstance();
        }

        /// <summary>
        /// To play the indicated sound
        /// </summary>
        /// <param name="sound">sound id</param>
        public void Play(int sound)
        {
            if (sound < 0 || sound > sounds.Length)
                return;

            sounds[sound].Play();
        }

        public void Stop(int sound)
        {
            if (sounds[sound] != null)
                sounds[sound].Stop();
        }

        /// <summary>
        /// Dispose all sound components
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < NUM_SOUNDS; i++)
            {
                if (sounds[i] != null)
                {
                    sounds[i].Dispose();
                    sounds[i] = null;
                }
            }
            instance = null;
        } 
        #endregion
    }
}
