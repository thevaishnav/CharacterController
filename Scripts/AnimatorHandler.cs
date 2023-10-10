﻿using System;
using UnityEngine;

namespace KS.CharaCon
{
    /// <summary> Handles animation controllers </summary>
    internal class AnimatorHandler
    {
        #region Statics
        public static readonly float MinMoveSpeed = 0.005f;
        public static readonly float SqreMinMoveSpeed = MinMoveSpeed * MinMoveSpeed;
        
        private static readonly int ID_AbilityIndex = Animator.StringToHash("Ability Index");
        private static readonly int ID_Speed = Animator.StringToHash("Speed");
        private static readonly int ID_IsMoving = Animator.StringToHash("Moving");
        private static readonly int ID_IsGrounded = Animator.StringToHash("Grounded");
        private static readonly int ID_ManagedFloat1 = Animator.StringToHash("ManagedFloat1");
        private static readonly int ID_ManagedFloat2 = Animator.StringToHash("ManagedFloat2");
        private static readonly int ID_ManagedInt1 = Animator.StringToHash("ManagedInt1");
        private static readonly int ID_ManagedInt2 = Animator.StringToHash("ManagedInt2");
        #endregion

        #region Fields and Properties
        private bool _isGrounded;
        private float _currentSpeed;
        private int _abilityIndex;
        private int _managedInt1;
        private int _managedInt2;
        private float _managedFloat1;
        private float _managedFloat2;
        private readonly Animator _animator;
        
        
        /// <summary> Is the player moving </summary>
        public bool IsMoving => _animator.GetBool(ID_IsMoving);

        /// <summary> Is the player touching the ground </summary>
        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                _animator.SetBool(ID_IsGrounded, value);
                _isGrounded = value;
            }
        }

        /// <summary> Movement speed of player </summary>
        public float Speed
        {
            get => this._currentSpeed;
            set
            {
                if (value > MinMoveSpeed)
                {
                    _animator.SetBool(ID_IsMoving, true);
                    _animator.SetFloat(ID_Speed, value);
                    _currentSpeed = value;
                }
                else
                {
                    _animator.SetBool(ID_IsMoving, false);
                    _animator.SetFloat(ID_Speed, 0f);
                    _currentSpeed = 0f;
                }
            }
        }

        /// <summary> Ability Index of currently enabled ability </summary>
        public int AbilityIndex
        {
            get => _abilityIndex;
            set
            {
                _animator.SetInteger(ID_AbilityIndex, value);
                _abilityIndex = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt1
        {
            get => _managedInt1;
            set
            {
                _animator.SetInteger(ID_ManagedInt1, value);
                _managedInt1 = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public int ManagedInt2
        {
            get => _managedInt2;
            set
            {
                _animator.SetFloat(ID_ManagedInt2, value);
                _managedInt2 = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat1
        {
            get => _managedFloat1;
            set
            {
                _animator.SetFloat(ID_ManagedFloat1, value);
                _managedFloat1 = value;
            }
        }

        /// <summary> Variable synchronised across all the devices (connected by photon) </summary>
        public float ManagedFloat2
        {
            get => _managedFloat2;
            set
            {
                _animator.SetFloat(ID_ManagedFloat2, value);
                _managedFloat2 = value;
            }
        }
        #endregion
        public AnimatorHandler(Animator animator)
        {
            _animator = animator;
            _currentSpeed = 0f;
            animator.SetFloat(ID_Speed, 0f);
            animator.SetBool(ID_IsMoving, false);
            /*controller.EvUpdate += UpdateSpeed;*/
        }

        /*private void UpdateSpeed()
        {
            if (Mathf.Abs(_currentSpeed - _targetSpeed) < MinMoveSpeed)
            {
                if (_currentSpeed < MinMoveSpeed)
                {
                    _currentSpeed = 0f;
                    _animator.SetFloat(ID_Speed, 0f);
                }

                return;
            }

            _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * 10f);
            _animator.SetFloat(ID_Speed, _currentSpeed);
        }*/
    }
}