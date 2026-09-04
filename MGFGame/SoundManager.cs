using System;
using System.Diagnostics;

namespace MGFGame
{
    public static class SoundManager
    {
        private static Music backgroundMusic;
        private static Sound clickSound;
        private static Sound jumpSound;
        private static Sound hitHurtSound;
        private static Sound pickupCoinSound;
        private static Sound swordSound;
        private static Sound laserShootSound;
        private static Sound deathSound;
        private static Sound bonusSound;
        private static bool isMusicPlaying = false;
        private static bool soundsLoaded = false;

        public static void LoadSounds()
        {
            Debug.WriteLine("=== STARTING TO LOAD SOUNDS ===");

            backgroundMusic = Engine.LoadMusic("music.wav");
            Debug.WriteLine("✓ Music loaded");

            Debug.WriteLine("Loading click.wav...");
            clickSound = Engine.LoadSound("click.wav");
            Debug.WriteLine("✓ Click loaded");

            Debug.WriteLine("Loading jump.wav...");
            jumpSound = Engine.LoadSound("jump.wav");
            Debug.WriteLine("✓ Jump loaded");

            Debug.WriteLine("Loading hitHurt.wav...");
            hitHurtSound = Engine.LoadSound("hitHurt.wav");
            Debug.WriteLine("✓ HitHurt loaded");

            Debug.WriteLine("Loading pickupCoin.wav...");
            pickupCoinSound = Engine.LoadSound("pickupCoin.wav");
            Debug.WriteLine("✓ PickupCoin loaded");

            Debug.WriteLine("Loading sword.wav...");
            swordSound = Engine.LoadSound("sword.wav");
            Debug.WriteLine("✓ Sword loaded");

            Debug.WriteLine("Loading laserShoot.wav...");
            laserShootSound = Engine.LoadSound("laserShoot.wav");
            Debug.WriteLine("✓ LaserShoot loaded");

            Debug.WriteLine("Loading death.wav...");
            deathSound = Engine.LoadSound("death.wav");
            Debug.WriteLine("✓ death loaded");

            Debug.WriteLine("Loading bonus.wav...");
            bonusSound = Engine.LoadSound("bonus.wav");
            Debug.WriteLine("✓ bonus loaded");

            Debug.WriteLine("=== ALL SOUNDS LOADED ===");
            soundsLoaded = true;
        }
        public static void PlayBackgroundMusic()
        {
            if (soundsLoaded && !isMusicPlaying && backgroundMusic != null)
            {
                Engine.PlayMusic(backgroundMusic, looping: true, fadeTime: 1.0f);
                isMusicPlaying = true;
                Debug.WriteLine("Background music started!");
            }
        }

        public static void PlayClick()
        {
            if (clickSound != null) Engine.PlaySound(clickSound, repeat: false, fadeTime: 0);
        }

        public static void PlayJump()
        {
            if (jumpSound != null) Engine.PlaySound(jumpSound, repeat: false, fadeTime: 0);
        }

        public static void PlayHitHurt()
        {
            if (hitHurtSound != null) Engine.PlaySound(hitHurtSound, repeat: false, fadeTime: 0);
        }

        public static void PlayPickupCoin()
        {
            if (pickupCoinSound != null) Engine.PlaySound(pickupCoinSound, repeat: false, fadeTime: 0);
        }

        public static void PlaySword()
        {
            if (swordSound != null) Engine.PlaySound(swordSound, repeat: false, fadeTime: 0);
        }

        public static void PlayLaserShoot()
        {
            if (laserShootSound != null) Engine.PlaySound(laserShootSound, repeat: false, fadeTime: 0);
        }


        //checking wrong sound, switched to deathSounds from ClickSounds
        public static void PlayDeath()
        {
            if (deathSound != null) Engine.PlaySound(deathSound, repeat: false, fadeTime: 0);
        }
        public static void StopBackgroundMusic()
        {
            if (isMusicPlaying)
            {
                Engine.StopMusic(fadeTime: 1.0f);
                isMusicPlaying = false;
            }
        }
    }
}