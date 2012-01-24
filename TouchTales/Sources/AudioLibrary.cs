/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Syderis.CellSDK.Core.Sounds; 
#endregion

namespace TouchyTales
{
    public class AudioLibrary
    {
        /// <summary>
        /// Constantes
        /// </summary>
        private const string path = "Sounds/";
        public const int SBALL = 0;        
        public const int SROPE = 1;
        public const int SDOLL1= 2;
        public const int SDOLL2 = 3;
        public const int SPIPIN = 4;        
        public const int STRAIN = 5;
        public const int SBALL2 = 6;

        /// <summary>
        /// Atributos
        /// </summary>
        private static AudioLibrary instance;
        private SoundInstance[] sounds;
        private const int NUM_SOUNDS = 7;

        /// <summary>
        /// Constructor privado (Singleton)
        /// </summary>
        private AudioLibrary()
        {
            sounds = new SoundInstance[NUM_SOUNDS];
        }

        /// <summary>
        /// Método que devuelve la única instancia de la clase
        /// Singleton.
        /// </summary>
        /// <returns></returns>
        public static AudioLibrary GetInstance()
        {
            if (instance == null)
                instance = new AudioLibrary();
            return instance;
        }

        /// <summary>
        /// Hace sonar el sonido indicado como argumento
        /// </summary>
        /// <param name="sound">Sonido</param>
        public void Play(int sound)
        {
            switch (sound)
	        {
		        case SBALL:
                    if (sounds[SBALL] == null)
                        sounds[SBALL] = Sound.CreateSound(path + "Treeball").CreateInstance();
                 break;                
                case SROPE:
                 if (sounds[SROPE] == null)
                     sounds[SROPE] = Sound.CreateSound(path + "RopeShort").CreateInstance();
                 break;
                case SDOLL1:
                 if (sounds[SDOLL1] == null)
                     sounds[SDOLL1] = Sound.CreateSound(path + "DollLove").CreateInstance();
                 break;
                case SDOLL2:
                 if (sounds[SDOLL2] == null)
                     sounds[SDOLL2] = Sound.CreateSound(path + "DollLaught").CreateInstance();
                 break;
                case SPIPIN:
                 if (sounds[SPIPIN] == null)
                     sounds[SPIPIN] = Sound.CreateSound(path + "Bird").CreateInstance();
                 break;
                case STRAIN:
                 if (sounds[STRAIN] == null)
                     sounds[STRAIN] = Sound.CreateSound(path + "ToyTrain").CreateInstance();
                 break;
                case SBALL2:
                 if (sounds[SBALL2] == null)
                     sounds[SBALL2] = Sound.CreateSound(path + "Ball").CreateInstance();
                 break;
            }

            sounds[sound].Play();
        }

        public void Stop(int sound)
        {
            if (sounds[sound] != null)
                sounds[sound].Stop();
        }

        /// <summary>
        /// Descarga todos los recursos reservados
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
    }
    
}
