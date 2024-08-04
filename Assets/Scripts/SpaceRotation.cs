using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

namespace SpaceTrucking {
    /// <summary>
    /// Add this ability to a character and it'll rotate or flip to face the direction of movement or the weapon's, or both, or none
    /// Only add this ability to a 2D character
    /// </summary>
    [MMHiddenProperties("AbilityStartFeedbacks", "AbilityStopFeedbacks")]
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Orientation 2D")]
    public class SpaceRotation : CharacterOrientation2D {

        protected override void DetermineFacingDirection() {
            if (_controller.CurrentDirection == Vector3.zero) {
                ApplyCurrentDirection();
            }

            if (_controller.CurrentDirection.normalized.magnitude >= AbsoluteThresholdMovement) {
                if (Mathf.Abs(_controller.CurrentDirection.y) > Mathf.Abs(_controller.CurrentDirection.x)) {
                    CurrentFacingDirection = (_controller.CurrentDirection.y > 0) ? Character.FacingDirections.North : Character.FacingDirections.South;
                } else {
                    CurrentFacingDirection = (_controller.CurrentDirection.x > 0) ? Character.FacingDirections.East : Character.FacingDirections.West;
                }
                _horizontalDirection = Mathf.Abs(_controller.CurrentDirection.x) >= AbsoluteThresholdMovement ? _controller.CurrentDirection.x : 0f;
                _verticalDirection = Mathf.Abs(_controller.CurrentDirection.y) >= AbsoluteThresholdMovement ? _controller.CurrentDirection.y : 0f;
            } else {
                _horizontalDirection = _lastDirectionX;
                _verticalDirection = _lastDirectionY;
            }

            switch (CurrentFacingDirection) {
                case Character.FacingDirections.West:
                    _directionFloat = 0f;
                    break;
                case Character.FacingDirections.North:
                    _directionFloat = 1f;
                    break;
                case Character.FacingDirections.East:
                    _directionFloat = 2f;
                    break;
                case Character.FacingDirections.South:
                    _directionFloat = 3f;
                    break;
            }

            _lastDirectionX = _horizontalDirection;
            _lastDirectionY = _verticalDirection;
        }

        /// <summary>
        /// Rotates the model in the specified direction
        /// </summary>
        /// <param name="direction"></param>
        protected override void RotateModel(int direction) {
            if (_character.CharacterModel != null) {
                //_targetModelRotation = (direction == 1) ? ModelRotationValueRight : ModelRotationValueLeft;
                //_targetModelRotation.x = _targetModelRotation.x % 360;
                //_targetModelRotation.y = _targetModelRotation.y % 360;
                //_targetModelRotation.z = _targetModelRotation.z % 360;




                Vector3 lastPosition = new Vector3(_lastDirectionX, 0, _lastDirectionY).normalized;
                Quaternion lastRotation = Quaternion.LookRotation(lastPosition);
                Quaternion currentRotation = Quaternion.LookRotation(_controller.CurrentDirection.normalized);
                _targetModelRotation = new Vector3(0, 0,Quaternion.RotateTowards(lastRotation, currentRotation, 5).eulerAngles.y * -1);
            }
        }
    }
}