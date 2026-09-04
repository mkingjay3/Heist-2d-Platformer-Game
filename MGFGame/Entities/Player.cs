using System;
using System.Collections.Generic;
using MGFGame;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using MGFCore.Engine;

namespace MGFGame.Entities
{
    public class Player : Entity
    {
        public Vector2 LastPosition { get; set; }
        public bool IsInvulnerable { get; set; } = false;
        public bool HasTripleShot { get; set; } = false;
        public float InvulnerabilityTimer { get; set; } = 0f;
        public float InvulnerabilityDuration { get; set; } = 2.0f;
        public float Speed { get; set; } = 6f;
        public float SlowdownTimer { get; set; } = 0f;
        public float ShieldRegenTimer { get; set; } = 0f;
        public float ShieldRegenTime { get; set; } = 10f;
        public bool IsJumping { get; set; } = false;
        public float JumpTimeHeld { get; set; } = 0f;
        public float MaxJumpTime { get; set; } = 0.4f;
        public float JumpBoostForce { get; set; } = -0.22f;
        public bool isFacingRight { get; set; } = true;

        public bool IsOnLadder { get; set; }
        public bool IsTouchingLadder { get; set; }

        public bool WantsToExitLadder { get; set; } = false;

        public float CurrentLadderTop { get; set; } = float.MinValue;

        public bool IsOnConveyer { get; set; }

        public bool IsOnMovingPlatform { get; set; }

        public int CollectedItems { get; set; } = 0;
        public bool IsAttacking { get; set; } = false;
        public float AttackTimer { get; set; } = 0f;
        public float AttackDuration { get; set; } = 0.5f;
        public float AttackCooldown { get; set; } = 1.0f;
        public float CooldownTimer { get; set; } = 0f;
        public Texture[] moveAnimation { get; set; }
        public Texture[] idleAnimation { get; set; }
        private Texture[] lastAnim;
        private bool wasGliding = false;
        public Texture[] shootAnimation { get; set; }
        public Texture[] climbAnimation { get; set; }
        public Texture[] glideAnimation { get; set; }
        public Texture[] fightAnimation { get; set; }
        public Texture[] slideAnimation { get; set; }
        public bool IsShooting { get; set; } = false;
        public float ShootingTimer { get; set; } = 0f;
        public float ShootingDuration { get; set; } = 0.3f;
        private Vector2 shootTarget;
        private bool bulletPending = false;
        public List<Projectile> ActiveProjectiles { get; set; } = new List<Projectile>();
        public float ShootCooldown { get; set; } = 0.05f;
        public float ShootCooldownTimer { get; set; } = 0f;
        public float ProjectileSpeed { get; set; } = 10f;
        public int MaxAmmo { get; set; } = 2;
        public int CurrentAmmo { get; set; } = 2;
        public float AmmoRechargeTimer { get; set; } = 0f;
        public float AmmoRechargeTime { get; set; } = 3f;
        public bool IsGliding { get; set; } = false;
        public float GlideGravity { get; set; } = 0.25f;
        public float MaxGlideSpeed { get; set; } = 3f;
        public float GlideTimer { get; set; } = 0f;
        public float MaxGlideTime { get; set; } = 2f;
        public bool IsSliding { get; set; } = false;
        public float SlideTimer { get; set; } = 0f;
        public float SlideDuration { get; set; } = 0.5f;
        public float SlideCooldown { get; set; } = 2.0f;  
        public float SlideCooldownTimer { get; set; } = 0f;
        public float SlideSpeed { get; set; } = 12f;
        public int SlideDirection { get; set; } = 1; 
        public Vector2 NormalSize { get; set; } = new Vector2(64, 100);
        public Vector2 SlideSize { get; set; } = new Vector2(54, 30);
        public bool IsTouchingWallLeft { get; set; } = false;
        public bool IsTouchingWallRight { get; set; } = false;
        public bool IsOnWall { get; set; } = false;
        public float WallSlideSpeed { get; set; } = 2f;  
        public float WallJumpForceX { get; set; } = 8f; 
        public float WallJumpForceY { get; set; } = -12f;
        public int LastWallJumpDirection { get; set; } = 0; 
        public bool NoClip { get; set; } = false;
        public int NextLevel { get; set; } = -1;
        public float wallJumpTimer { get; set; } = 0f;
        public Player(Vector2 startPosition) : base()
        {
            Position = startPosition;
            Velocity = Vector2.Zero;
            IsOnGround = false;
            Size = new Vector2(64, 100);
            Health = 5;
            Armor = 3;
        }
        public void Jump()
        {
            if (IsOnGround || IsOnLadder)
            {
                if (IsOnLadder) ExitLadder();
                Velocity = new Vector2(Velocity.X, -13f);
                IsOnGround = false;
                IsJumping = true;
                JumpTimeHeld = 0f;
                SoundManager.PlayJump();
            }
        }

        public override void takeDamage()
        {
            if (!IsInvulnerable && damageTimer <= 0f)
            {
                if (Armor > 0)
                {
                    Armor -= 1;
                }
                else
                {
                    Health -= 1;
                }
                isDamaged = true;
                damageTimer = 0.25f;

                IsInvulnerable = true;
                InvulnerabilityTimer = InvulnerabilityDuration;
            }
        }

        public void ContinueJump()
        {
            if (IsJumping && JumpTimeHeld < MaxJumpTime && Velocity.Y < 0)
            { 
                Velocity = new Vector2(Velocity.X, Velocity.Y + JumpBoostForce);
                JumpTimeHeld += 0.016f;
            }
        }
        public void EndJump()
        {
            IsJumping = false;
        }

        public void MoveLeft()
        {
            if (wallJumpTimer > 0) return; // Prevent overriding wall jump velocity
            float currentSpeed = SlowdownTimer > 0 ? Speed * 0.5f : Speed;
            Velocity = new Vector2(-currentSpeed, Velocity.Y);
            isFacingRight = false;

        }

        public void MoveRight()
        {
            if (wallJumpTimer > 0) return; // Prevent overriding wall jump velocity
            float currentSpeed = SlowdownTimer > 0 ? Speed * 0.5f : Speed;
            Velocity = new Vector2(currentSpeed, Velocity.Y);
            isFacingRight = true;
        }

        public void MoveUp()
        {
            Velocity = new Vector2(Velocity.X, -Speed);
        }

        public void MoveDown()
        {
            Velocity = new Vector2(Velocity.X, Speed);
        }

        public void ClimbUp()
        {
            if (IsOnLadder)
            {
                float playerFeet = Position.Y + Size.Y;
                if (playerFeet <= CurrentLadderTop + 10)
                {
                    IsOnLadder = false;
                    Velocity = new Vector2(Velocity.X, -Speed); // Boost up to clear the top
                    return;
                }
                Velocity = new Vector2(0, -Speed);
            }
        }

        public void ClimbDown()
        {
            if (IsOnLadder)
                Velocity = new Vector2(0, Speed);
        }
        public void ExitLadder()
        {
            IsOnLadder = false;
            WantsToExitLadder = true;
        }

        public void Melee()
        {
            if (!IsAttacking && CooldownTimer <= 0f)
            {
                IsAttacking = true;
                AttackTimer = 0f;
                if (fightAnimation != null)
                {
                    AttackDuration = (fightAnimation.Length) * 0.1f + 0.05f;
                }
            }
        }

        public void Shoot(Vector2 target)
        {
            if (CurrentAmmo > 0 && ShootCooldownTimer <= 0f)
            {
                if (target.X < Position.X)
                    isFacingRight = false;
                else if (target.X > Position.X)
                    isFacingRight = true;

                shootTarget = target;
                bulletPending = true;
                IsShooting = true;
                ShootingTimer = 0f;
                ShootCooldownTimer = ShootCooldown;
                if (shootAnimation != null)
                {
                    ShootingDuration = (shootAnimation.Length) * 0.1f + 0.05f;
                }
            }
        }

        private void SpawnBullet(Vector2 target)
        {
            Vector2 center = Position + Size / 2;
            Vector2 direction = target - center;
            float length = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (length > 0)
            {
                Vector2 normDirection = new Vector2(direction.X / length, direction.Y / length);
                Vector2 projectileVelocity = new Vector2(normDirection.X * ProjectileSpeed, normDirection.Y * ProjectileSpeed);
                Vector2 spawnPosition = center;
                float rotationRadians = (float)Math.Atan2(direction.Y, direction.X);
                float rotationDegrees = rotationRadians * (180f / (float)Math.PI);

                if (HasTripleShot)
                {
                    // Fire 3 bullets in a cone shapeish
                    float[] angles = { rotationRadians - 0.1f, rotationRadians, rotationRadians + 0.1f };
                    foreach (float angle in angles)
                    {
                        Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * ProjectileSpeed;
                        Projectile p = new Projectile(spawnPosition, vel, angle * (180f / (float)Math.PI), 5.0f);
                        ActiveProjectiles.Add(p);
                    }
                }
                else
                {
                    Projectile projectile = new Projectile(spawnPosition, projectileVelocity, rotationDegrees, 5.0f);
                    ActiveProjectiles.Add(projectile);
                }
            }
        }

        public void StartGlide()
        {

            //you can glide even when falling now

            if (!IsOnGround && !IsOnLadder && GlideTimer < MaxGlideTime)
            {
                if (!IsGliding)
                {
                    Velocity = new Vector2(Velocity.X, 0); // Cancel vertical velocity immediately
                }
                IsGliding = true;
            }
            else if (GlideTimer >= MaxGlideTime)
            {
                IsGliding = false;
            }
        }

        public void StopGlide()
        {
            IsGliding = false;
        }

        public void StartSlide()
        {
            if (IsOnGround && !IsSliding && SlideCooldownTimer <= 0f)
            {
                IsSliding = true;
                SlideTimer = 0f;

                if (isFacingRight)
                    SlideDirection = 1;
                else
                    SlideDirection = -1;

                float heightDiff = NormalSize.Y - SlideSize.Y;
                Position = new Vector2(Position.X, Position.Y + heightDiff);
                Size = SlideSize;
            }
        }

        public void StopSlide()
        {
            if (IsSliding)
            {
                IsSliding = false;
                SlideCooldownTimer = SlideCooldown; 

                float heightDiff = NormalSize.Y - SlideSize.Y;
                Position = new Vector2(Position.X, Position.Y - heightDiff);
                Size = NormalSize;
            }
        }

        public void WallJump()
        {
            if (!IsOnGround && IsOnWall && !IsOnLadder && !IsTouchingLadder)
            {
                if (IsTouchingWallLeft)
                {
                    Velocity = new Vector2(WallJumpForceX, WallJumpForceY);
                    isFacingRight = true;
                    wallJumpTimer = 0.3f; // Lock horizontal movement for a bit
                    SoundManager.PlayJump();
                }
                else if (IsTouchingWallRight)
                {
                    Velocity = new Vector2(-WallJumpForceX, WallJumpForceY);
                    isFacingRight = false;
                    wallJumpTimer = 0.3f; // Lock horizontal movement for a bit
                    SoundManager.PlayJump();
                }

                IsOnWall = false;
                IsTouchingWallLeft = false;
                IsTouchingWallRight = false;
            }
        }

        public void Update()
        {
            if (SlowdownTimer > 0)
            {
                SlowdownTimer -= 0.016f;
            }
            if (damageTimer > 0f)
            {
                damageTimer -= 0.016f;
            }
            else
            {
                isDamaged = false;
            }
            LastPosition = Position;

            if (InvulnerabilityTimer > 0f)
            {
                InvulnerabilityTimer -= 0.016f;
            }
            else
            {
                IsInvulnerable = false;
            }

            if (CurrentAmmo < MaxAmmo)
            {
                AmmoRechargeTimer += 0.016f;
                if (AmmoRechargeTimer >= AmmoRechargeTime)
                {
                    CurrentAmmo++;
                    AmmoRechargeTimer = 0f;
                }
            }

            //make it so that you can regain ur shield
            if (Armor < 3)
            {
                ShieldRegenTimer += 0.016f;
                if (ShieldRegenTimer >= ShieldRegenTime)
                {
                    Armor++;
                    ShieldRegenTimer = 0f;
                }
            }
            else
            {
                ShieldRegenTimer = 0f;
            }

            FrameTimer +=0.016f;

            float frameThreshold = 0.15f;
            if (IsAttacking || IsShooting) frameThreshold = 0.1f;

            if (FrameTimer >= frameThreshold)
            {
                Texture[] currentAnim = idleAnimation;
                IsMoving = Velocity.X != 0 || Velocity.Y != 0;

                if (IsAttacking)
                {
                    currentAnim = fightAnimation;
                }
                else if (IsShooting)
                {
                    currentAnim = shootAnimation;
                }
                else if (IsSliding)
                {
                    currentAnim = slideAnimation;
                }
                else if (IsOnLadder)
                {
                    currentAnim = climbAnimation;
                }
                else if (IsGliding)
                {
                    currentAnim = glideAnimation;
                }
                else if (!IsOnGround && Velocity.Y != 0)
                {
                    currentAnim = idleAnimation;
                }
                else if (Velocity.X != 0)
                {
                    currentAnim = moveAnimation;
                }

                bool animationChanged = currentAnim != lastAnim;
                bool glideStarted = currentAnim == glideAnimation && IsGliding && !wasGliding;
                bool glideEnded = currentAnim == glideAnimation && !IsGliding && wasGliding;

                if (animationChanged || glideStarted || glideEnded)
                {
                    if (currentAnim == glideAnimation && !IsGliding && !glideEnded)
                    {
                        frame = 1; 
                    }
                    else
                    {
                        frame = 0;
                    }
                    lastAnim = currentAnim;
                }
                else
                {
                    if (currentAnim != null && currentAnim.Length > 0)
                    {
                        frame++;
                        if (currentAnim == glideAnimation)
                        {
                            if (frame >= currentAnim.Length)
                            {
                                frame = 1; 
                            }
                        }
                        else
                        {
                            if (frame >= currentAnim.Length)
                            {
                                frame = 0;
                            }
                        }
                    }
                }

                if (currentAnim != null && currentAnim.Length > 0)
                {
                    currentTexture = currentAnim[frame];
                }
                FrameTimer = 0f;
                wasGliding = IsGliding;
            }
            
            if (IsShooting && bulletPending)
            {
                if (frame == shootAnimation.Length - 1)
                {
                    SpawnBullet(shootTarget);
                    bulletPending = false;
                    CurrentAmmo--;
                }
            }

            if (SlideCooldownTimer > 0)
            {
                SlideCooldownTimer -= 0.016f;
            }
            if (IsSliding)
            {
                SlideTimer += 0.016f;

                Velocity = new Vector2(SlideDirection * SlideSpeed, Velocity.Y);

                if (SlideTimer >= SlideDuration)
                {
                    StopSlide();
                }
            }

            if (!IsOnLadder && !NoClip)
            {
                if (IsGliding)
                {
                    GlideTimer += 0.016f;

                    if (GlideTimer >= MaxGlideTime)
                    {
                        IsGliding = false;
                    }
                    else
                    {
                        Velocity = new Vector2(Velocity.X, Velocity.Y + GlideGravity);
                        if (Velocity.Y > MaxGlideSpeed)
                        {
                            Velocity = new Vector2(Velocity.X, MaxGlideSpeed);
                        }
                    }
                }
                else if (IsOnWall && !IsOnGround && Velocity.Y > 0)
                {
                    Velocity = new Vector2(Velocity.X, WallSlideSpeed);
                }
                else
                {
                    Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
                }
            }

            Position += Velocity;

            if (IsOnLadder || NoClip)
            {
             
                Velocity = Vector2.Zero;
            }
            else
            {
                if (wallJumpTimer > 0)
                {
                    wallJumpTimer -= 0.016f;
                    // Maintain horizontal velocity during wall jump
                }
                else
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }

            IsOnConveyer = false;
            IsOnMovingPlatform = false;
            IsOnWall = false;
            IsTouchingWallLeft = false;
            IsTouchingWallRight = false;

            if (IsOnGround)
            {
                IsGliding = false;
                LastWallJumpDirection = 0;
                GlideTimer = 0f;
                wallJumpTimer = 0f;
            }

            if (!IsOnGround && IsSliding)
            {
                StopSlide();
            }


            if (IsAttacking)
            {
                AttackTimer += 0.016f;

                if(AttackTimer >= AttackDuration)
                {
                    IsAttacking = false;
                    CooldownTimer = AttackCooldown;
                }
            }

            if(CooldownTimer > 0)
            {
                CooldownTimer -= 0.016f;
            }

            if (IsShooting)
            {
                ShootingTimer += 0.016f;
                if (ShootingTimer >= ShootingDuration)
                {
                    IsShooting = false;
                    bulletPending = false;
                }
            }

            if(ShootCooldownTimer > 0)
            {
                ShootCooldownTimer -= 0.016f;
            }
            
            for(int i = ActiveProjectiles.Count - 1; i >= 0; i--)
            {
                ActiveProjectiles[i].Update();

                if(!ActiveProjectiles[i].IsAlive())
                {
                    ActiveProjectiles.RemoveAt(i);
                }
            }
        }

        public override Bounds2 GetBounds()
        {
            if (IsSliding)
            {
                return new Bounds2(Position, Size);
            }
            
            // For all other states (including melee and shooting), use the normal height
            // to prevent entering 1-tile high spaces.
            return new Bounds2(Position, NormalSize);
        }

        public void Draw(Vector2 screenPosition)
        {
            Debug.WriteLine(LastPosition);
            if (currentTexture != null)
            {
                float scale = 1.0f;
                if (idleAnimation != null && idleAnimation.Length > 0)
                {
                    scale = NormalSize.Y / idleAnimation[0].Height;
                }

                Vector2 drawSize = new Vector2(currentTexture.Width * scale, currentTexture.Height * scale);
                Vector2 drawPos = screenPosition;

                if (drawSize.Y > Size.Y)
                {
                    drawPos.Y -= (drawSize.Y - Size.Y);
                }
                
                if (drawSize.X > Size.X)
                {
                    if (!isFacingRight)
                    {
                        drawPos.X -= (drawSize.X - Size.X);
                    }
                }

                if (!isFacingRight)
                {
                    if (isDamaged)
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest, mirror: TextureMirror.Horizontal, color: new Color(255, 0, 0, 255));
                    }
                    else
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest, mirror: TextureMirror.Horizontal);
                    }
                }
                else
                {
                    if (isDamaged)
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest, color: new Color(255, 0, 0, 255));
                    }
                    else
                    {
                        Engine.DrawTexture(currentTexture, drawPos, size: drawSize, scaleMode: TextureScaleMode.Nearest);
                    }

                }
            }

        }


        //added code to create a bound to make sure player isnt pushed under map 
        //playtested feedback for level 3
        public override void touch(Entity entity, Bounds2 e, Bounds2 p)
        {

            if(entity is OneShotEnemy)
            {
                SlowdownTimer = 1.5f;
                return;
            }


            float left = e.Right - p.Left;
            float right = p.Right - e.Left;
            float top = e.Bottom - p.Top;
            float bottom = p.Bottom - e.Top;
            float min = Math.Min(Math.Min(left, right), Math.Min(top, bottom));

            if (min == top)
            {
                Position = new Vector2(Position.X, p.Top - Size.Y);
                if (Velocity.Y > 0)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

            else if (min == bottom)
            {
                Position = new Vector2(Position.X, p.Bottom);
                if (Velocity.Y < 0)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }

            else if (min == left)
            {
                Position = new Vector2(p.Left - Size.X, Position.Y);
                if (Velocity.X > 0)
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }

            else if (min == right)
            {
                Position = new Vector2(p.Right, Position.Y);
                if (Velocity.X < 0)
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }
        }
    }
}
